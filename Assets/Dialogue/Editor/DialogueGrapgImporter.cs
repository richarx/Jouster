using System;
using System.Collections.Generic;
using System.Linq;
using Map;
using Unity.GraphToolkit.Editor;
using UnityEditor.AssetImporters;
using UnityEngine;

[ScriptedImporter(1, DialogueGraph.AssetExtension)]
public class DialogueGrapgImporter : ScriptedImporter
{
    public override void OnImportAsset(AssetImportContext ctx)
    {
        DialogueGraph editorGraph = GraphDatabase.LoadGraphForImporter<DialogueGraph>(ctx.assetPath);
        RuntimeDialogueGraph runtimeGraph = ScriptableObject.CreateInstance<RuntimeDialogueGraph>();

        Dictionary<INode, string> nodeIDMap = new Dictionary<INode, string>();

        foreach (INode node in editorGraph.GetNodes())
        {
            nodeIDMap[node] = Guid.NewGuid().ToString();
        }

        INode startNode = editorGraph.GetNodes().OfType<StartNode>().FirstOrDefault();
        if (startNode != null)
        {
            IPort entryPort = startNode.GetOutputPorts().FirstOrDefault()?.FirstConnectedPort;
            if (entryPort != null)
            {
                runtimeGraph.EntryNodeID = nodeIDMap[entryPort.GetNode()];
            }
        }

        foreach (INode node in editorGraph.GetNodes())
        {
            if (node is StartNode || node is EndNode)
                continue;

            RuntimeDialogueNode runtimeNode = new RuntimeDialogueNode { NodeID = nodeIDMap[node] };
            if (node is DialogueNode dialogueNode)
            {
                ProcessDialogueNode(dialogueNode, runtimeNode, nodeIDMap);
            }
            else if (node is ChoiceNode choiceNode)
            {
                ProcessChoiceNode(choiceNode, runtimeNode, nodeIDMap);
            }
            else if (node is EventNode eventNode)
            {
                ProcessEventNode(eventNode, runtimeNode, nodeIDMap);
            }

            runtimeGraph.AllNodes.Add(runtimeNode);
        }

        ctx.AddObjectToAsset("RuntimeData", runtimeGraph);
        ctx.SetMainObject(runtimeGraph);
    }

    private void ProcessDialogueNode(DialogueNode node, RuntimeDialogueNode runtimeNode, Dictionary<INode, string> nodeIDMap)
    {
        runtimeNode.SpeakerName = GetPortValue<string>(node.GetInputPortByName("Speaker"));
        runtimeNode.DialogueText = GetPortValue<string>(node.GetInputPortByName("Dialogue"));

        IPort nextNodePort = node.GetOutputPortByName("out")?.FirstConnectedPort;
        if (nextNodePort != null)
            runtimeNode.NextNodeID = nodeIDMap[nextNodePort.GetNode()];
    }

    private void ProcessChoiceNode(ChoiceNode node, RuntimeDialogueNode runtimeNode, Dictionary<INode, string> nodeIDMap)
    {
        runtimeNode.SpeakerName = GetPortValue<string>(node.GetInputPortByName("Speaker"));
        runtimeNode.DialogueText = GetPortValue<string>(node.GetInputPortByName("Dialogue"));

        IEnumerable<IPort> choiceOutputPorts = node.GetOutputPorts().Where((p) => p.Name.StartsWith("Choice "));

        foreach (var outputPort in choiceOutputPorts)
        {
            string index = outputPort.Name.Substring("Choice ".Length);
            IPort textPort = node.GetInputPortByName($"Choice Text {index}");

            ChoiceData choiceData = new ChoiceData
            {
                ChoiceText = GetPortValue<string>(textPort),
                DestinationNodeID = outputPort.FirstConnectedPort != null ? nodeIDMap[outputPort.FirstConnectedPort.GetNode()] : null
            };

            runtimeNode.Choices.Add(choiceData);
        }
    }

    private void ProcessEventNode(EventNode node, RuntimeDialogueNode runtimeNode, Dictionary<INode, string> nodeIDMap)
    {
        runtimeNode.dialogueEvent = GetPortValue<DialogueEvent>(node.GetInputPortByName("EventType"));

        IPort nextNodePort = node.GetOutputPortByName("out")?.FirstConnectedPort;
        if (nextNodePort != null)
            runtimeNode.NextNodeID = nodeIDMap[nextNodePort.GetNode()];
    }

    private T GetPortValue<T>(IPort port)
    {
        if (port == null)
            return default;

        if (port.IsConnected)
        {
            if (port.FirstConnectedPort.GetNode() is IVariableNode variableNode)
            {
                variableNode.Variable.TryGetDefaultValue(out T value);
                return value;
            }
        }

        port.TryGetValue(out T fallbackValue);
        return fallbackValue;
    }
}
