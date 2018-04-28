using Harmony;

namespace MaxAttributes
{
    [HarmonyPatch(typeof(Database.AttributeConverters), "Create")]
    public static class MaxAttributes
    {
        public static void Prefix(ref string id, ref string name, ref string description, ref Klei.AI.Attribute attribute, ref float multiplier, ref float base_value, ref IAttributeFormatter formatter)
        {
            //IDs to ignore
            //id: ImmuneLevelBoost
            //id: TemperatureInsulation
            //id: SeedHarvestChance

            if (id != "ImmuneLevelBoost" && id != "TemperatureInsulation" && id != "SeedHarvestChance")
            {
                Debug.Log(" === Attribute " + id + " boosted === ");
                multiplier = 1000.0f;
                base_value = 1000.0f;
            }

            /*Debug.Log("id: " + id.ToString());
            Debug.Log("name: " + name.ToString());
            Debug.Log("description: " + description.ToString());
            Debug.Log("attribute: " + attribute.ToString());
            Debug.Log("multiplier: " + multiplier.ToString());
            Debug.Log("base_value: " + base_value.ToString());
            Debug.Log("formatter: " + formatter.ToString());
            Debug.Log(" === MaxAttributes END === ");*/
        }
    }
}