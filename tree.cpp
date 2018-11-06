// node for tree
struct TreeNode
{
	bool test(int value);   // The test to perform in this node.
	TreeNode *left;   // Pointer to the left subtree.
	TreeNode *right;  // Pointer to the right subtree.
}

// Leaf node
struct LeafNode:TreeNode
{
	Action* actionToPerform;
}

//the decision tree class
class DecisionTree
{
public:
    //functions
    DecisionTree();
    Action* MakeDecision(TreeNode* node);

private:
    TreeNode* m_RootNode;
};

// Recursively work down the tree until a leaf is reached
Action* DecisionTree::MakeDecision(TreeNode* node, int value)
{
	// Perform the test to decide which way to go
	if(test(value))
	{
		// go down left branch if there is one
		if(node.left != NULL)
			MakeDecision(node.left);
		else
		{
			// otherwise we've reached a leaf node. Get the action
			LeafNode* leaf = static_cast<LeafNode*>(node)
			return leaf.actionToPerform
		}
	}
	else
	{
		// go down right branch if there is one
		if(node.right != NULL)
			MakeDecision(node.right);
		else
		{
			// otherwise we've reached a leaf node. Get the action
			LeafNode* leaf = static_cast<LeafNode*>(node)
			return leaf.actionToPerform
		}
	}
}






