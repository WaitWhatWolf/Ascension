using Ascension.Enums;

namespace Ascension.Players
{
    /// <summary>
    /// Core record for Stands.
    /// </summary>
    public sealed record Stand
    {
        public string Name { get; }
        public StandID ID { get; }

        public Stand(StandID id)
        {
            ID = id;
            Name = ASCResources.Players.GetStandName(ID);
        }
    
        public static implicit operator bool(Stand stand) => stand != null && stand.ID != StandID.NEWBIE;
    }
}
