using UnityEditor.IMGUI.Controls;

namespace LDtkUnity.Editor
{
    internal sealed class LDtkTreeViewProject : LDtkTreeView
    {
        private readonly LdtkJson _json;
        private readonly string _projectName;

        public LDtkTreeViewProject(TreeViewState<int> state, LdtkJson json, string projectName) : base(state)
        {
            _json = json;
            _projectName = projectName;
        }

        protected override void BuildFirstRoot(TreeViewItem<int> parent)
        {
            BuildProject(parent);
        }

        private void BuildProject(TreeViewItem<int> root)
        {
            TreeViewItem<int> projectItem = CreateTreeItem(true);
            projectItem.displayName = _projectName;
            projectItem.icon = LDtkIconUtility.LoadListIcon();
            root.AddChild(projectItem);

            BuildWorlds(projectItem);
        }

        private void BuildWorlds(TreeViewItem<int> projectItem)
        {
            Depth++;
            foreach (World world in _json.UnityWorlds)
            {
                BuildWorld(projectItem, world);
            }
        }

        private void BuildWorld(TreeViewItem<int> parent, World world)
        {
            TreeViewItem<int> worldItem = CreateTreeItem(false);
            worldItem.displayName = world.Identifier;
            worldItem.icon = LDtkIconUtility.LoadWorldIcon();
            parent.AddChild(worldItem);

            BuildLevels(worldItem, world);
        }

        private void BuildLevels(TreeViewItem<int> worldItem, World world)
        {
            Depth++;
            foreach (Level level in world.Levels)
            {
                BuildLevel(worldItem, level);
            }
        }
    }
}