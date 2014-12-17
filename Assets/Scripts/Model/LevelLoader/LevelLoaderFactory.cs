using TrappedGame.Model.LevelLoader.Ascii;
using TrappedGame.Model.LevelLoader.Json;

using UnityEngine;
using System.IO;


// TODO Delete this hack after deleting Ascii format
namespace TrappedGame.Model.LevelLoader {
	public class LevelLoaderFactory {
		public static ILevelLoader GetLoader(string fileName) {
            if (IsJson(fileName)) {
				return new JsonLevelLoader ();
			}
			return new AsciiLevelLoader();
		}

		static bool IsJson(string fileName) {
			var text = Resources.Load<TextAsset>(fileName);
			var stream = new MemoryStream(text.bytes);
			var reader = new StreamReader(stream);

		    var firstSymbol = (char) reader.Read();

            reader.Close();
            stream.Close();

            return firstSymbol == '{';
		}
	}
}