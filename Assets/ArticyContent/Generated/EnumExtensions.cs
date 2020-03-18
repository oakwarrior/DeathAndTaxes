namespace Articy.Project_Of_Death
{
	public static class EnumExtensionMethods
	{
		public static string GetDisplayName(this item_accessory_type aitem_accessory_type)
		{
			return Articy.Unity.ArticyTypeSystem.GetArticyType("item_accessory_type").GetEnumValue(((int)(aitem_accessory_type))).DisplayName;
		}

		public static string GetDisplayName(this profile_generate_spare_death_both aprofile_generate_spare_death_both)
		{
			return Articy.Unity.ArticyTypeSystem.GetArticyType("profile_generate_spare_death_both").GetEnumValue(((int)(aprofile_generate_spare_death_both))).DisplayName;
		}

		public static string GetDisplayName(this profile_occupation_selector aprofile_occupation_selector)
		{
			return Articy.Unity.ArticyTypeSystem.GetArticyType("profile_occupation_selector").GetEnumValue(((int)(aprofile_occupation_selector))).DisplayName;
		}

		public static string GetDisplayName(this item_type_category aitem_type_category)
		{
			return Articy.Unity.ArticyTypeSystem.GetArticyType("item_type_category").GetEnumValue(((int)(aitem_type_category))).DisplayName;
		}

		public static string GetDisplayName(this task_death_occupation_selector atask_death_occupation_selector)
		{
			return Articy.Unity.ArticyTypeSystem.GetArticyType("task_death_occupation_selector").GetEnumValue(((int)(atask_death_occupation_selector))).DisplayName;
		}

		public static string GetDisplayName(this task_spare_occupation_selector atask_spare_occupation_selector)
		{
			return Articy.Unity.ArticyTypeSystem.GetArticyType("task_spare_occupation_selector").GetEnumValue(((int)(atask_spare_occupation_selector))).DisplayName;
		}

		public static string GetDisplayName(this ShapeType aShapeType)
		{
			return Articy.Unity.ArticyTypeSystem.GetArticyType("ShapeType").GetEnumValue(((int)(aShapeType))).DisplayName;
		}

		public static string GetDisplayName(this SelectabilityModes aSelectabilityModes)
		{
			return Articy.Unity.ArticyTypeSystem.GetArticyType("SelectabilityModes").GetEnumValue(((int)(aSelectabilityModes))).DisplayName;
		}

		public static string GetDisplayName(this VisibilityModes aVisibilityModes)
		{
			return Articy.Unity.ArticyTypeSystem.GetArticyType("VisibilityModes").GetEnumValue(((int)(aVisibilityModes))).DisplayName;
		}

		public static string GetDisplayName(this OutlineStyle aOutlineStyle)
		{
			return Articy.Unity.ArticyTypeSystem.GetArticyType("OutlineStyle").GetEnumValue(((int)(aOutlineStyle))).DisplayName;
		}

		public static string GetDisplayName(this PathCaps aPathCaps)
		{
			return Articy.Unity.ArticyTypeSystem.GetArticyType("PathCaps").GetEnumValue(((int)(aPathCaps))).DisplayName;
		}

		public static string GetDisplayName(this LocationAnchorSize aLocationAnchorSize)
		{
			return Articy.Unity.ArticyTypeSystem.GetArticyType("LocationAnchorSize").GetEnumValue(((int)(aLocationAnchorSize))).DisplayName;
		}

	}
}

