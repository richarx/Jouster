using UnityEditor.IMGUI.Controls;

namespace LDtkUnity.Editor
{
    internal sealed class LDtkTreeViewLevel : LDtkTreeView
    {
        private readonly Level _json;

        public LDtkTreeViewLevel(TreeViewState<int> state, Level json) : base(state)
        {
            _json = json;
        }

        protected override void BuildFirstRoot(TreeViewItem<int> parent)
        {
            BuildLevel(parent, _json);
        }
    }
}