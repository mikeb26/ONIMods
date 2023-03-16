namespace PeterHan.PLib.OptionsFilt;

public class Util {
	// @todo should probably move to PLibCore?
	// Returns true if the specified mod is installed & enabled
	public static bool IsModEnabled(string staticID) {
		foreach(var mod in Global.Instance.modManager.mods) {
			if (mod.staticID == staticID && mod.IsActive()) {
				return true;
			}
		}

		return false;
	}
}
