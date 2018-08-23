using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

public class TestWormController : MonoBehaviour {

	void Awake () {
		BehaviorTree tree = gameObject.AddComponent<BehaviorTree> ();
		tree.StartWhenEnabled = false;

		EntryTask entryTask = new EntryTask ();
		entryTask.NodeData = new NodeData ();
		tree.GetBehaviorSource ().EntryTask = entryTask;

		Sequence rootTask = new Sequence ();
        rootTask.NodeData = new NodeData();

		tree.GetBehaviorSource ().RootTask = rootTask;

		Wait myWait = new Wait ();
		myWait.NodeData = new NodeData ();
		rootTask.AddChild (myWait, 0);

		PatrollTask patroll = new PatrollTask ();
		patroll.NodeData = new NodeData ();
		rootTask.AddChild (patroll, 1);

		tree.GetBehaviorSource ().HasSerialized = true;
		tree.EnableBehavior ();

	}

}
