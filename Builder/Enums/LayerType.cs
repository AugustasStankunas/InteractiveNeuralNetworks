using System.ComponentModel;

namespace Builder.Enums
{
    public enum LayerType
    {
        [Description("Accepts and holds the raw input features, optionally normalizing them")]
        Input,

        [Description("Combines all learned features to produce the final predictions or outputs")]
        Output,

        [Description("A fully-connected (dense) layer that learns weighted connections between every input and output neuron")]
        FullyConnected,

        [Description("A convolutional layer that applies sliding filters to detect spatial patterns in the input")]
        Convolutional,

        [Description("A pooling layer that reduces spatial dimensions by summarizing local neighborhoods")]
        Pooling,

        [Description("Normalizes activations over a mini-batch to stabilize and accelerate training")]
        BatchNorm,

        [Description("Flattens multi-dimensional input tensors into a single vector for downstream layers")]
        Flatten,

        [Description("Randomly sets a fraction of input units to zero at each update, reducing overfitting")]
        Dropout,

        [Description("Element-wise addition of two input tensors, often used in residual connections")]
        Add,

        [Description("Connects 2 layers, allowing data flow from the source to the target layer")]
        Connection,

        [Description("Default item, something has gone wrong")]
        Default,
    }
}
