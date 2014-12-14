using TrappedGame.Model.LevelLoader.Ascii;
using TrappedGame.Model.LevelLoader.Json;

using UnityEngine;
using System.IO;


namespace TrappedGame.Model.LevelLoader {
	public class LevelLoaderFactory {
		public static ILevelLoader GetLoader(string fileName) {
			if (isJson (fileName)) {
				return new JsonLevelLoader ();
			} else {
				return new AsciiLevelLoader();
			}
		}

		static bool isJson(string fileName) {
			var text = Resources.Load<TextAsset>(fileName);
			var stream = new MemoryStream(text.bytes);
			var reader = new StreamReader(stream);

			return (char)reader.Read() == '{';
		}
	}
}