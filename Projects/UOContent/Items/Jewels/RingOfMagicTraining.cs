using ModernUO.Serialization;

namespace Server.Items;

[SerializationGenerator(0, false)]
public partial class RingOfMagicTraining : GoldRing
{
    public override string DefaultName => "ring of magic training";

    [Constructible]
    public RingOfMagicTraining()
    {
        Attributes.LowerRegCost = 100;
    }
}
