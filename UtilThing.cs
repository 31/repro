using Godot;
using System;
using System.Collections.Generic;

namespace GodotCsIsAssignableFrom
{
	public class Utilities
	{
		public static List<Type> barebones = new List<Type>()
		{
			typeof(CollisionObject),
			typeof(CollisionShape),
			typeof(CollisionPolygon),
			typeof(Navigation),
			typeof(NavigationMesh),
			typeof(NavigationMeshInstance),
			typeof(NavigationPolygon),
			typeof(NavigationPolygonInstance),
			typeof(PhysicsBody),
			typeof(Area),
			typeof(RayCast),
			typeof(RayShape),
		};

		public static List<string> s = new();

		public static void Log(string m) => GD.Print(string.Concat(s), m);

		public static int Children_StripVisuals(Node root)
		{
			s.Add("  ");

			Log("Base " + root.GetPath());
			// Counter for deleted children.
			int children = 0;

			foreach (Node n in root.GetChildren())
			{
				s.Add("  ");
				Log("Iterating " + n.GetPath());
				bool validNode = false;
				Type nodeType = n.GetType();

				foreach (Type t in barebones)
				{
					if (t.IsAssignableFrom(nodeType) || t.Equals(nodeType))
					{
						validNode = true;
						break;
					}
				}

				Log("Valid: " + validNode);

				if (validNode)
				{
					// Apparently the `Owner` property of a Node must be set for PackedScene to save children.
					// We set the `Owner` to the direct parent to keep the same hierarchy.
					n.Owner = n.GetParent();
					children += Children_StripVisuals(n);
				}
				else
				{
					int recur = Children_StripVisuals(n);
					int nodeChildren = n.GetChildren().Count;

					children += recur;

					if (nodeChildren == 0)
					{
						Log("----> Would free " + n.GetPath());
						// n.Free();
						children++;
					}
				}

				Log("Iterating... done");
				s.RemoveAt(s.Count - 1);
			}

			Log("Base... done");
			s.RemoveAt(s.Count - 1);
			return children;
		}
	}
}
