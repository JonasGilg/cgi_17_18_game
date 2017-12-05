using System.Collections.Generic;
using Engine;
using NUnit.Framework;
using OpenTK;
using AABB = Engine.AxisAlignedBoundingBox;

namespace EngineTest.Util.DataStructure {
	[TestFixture]
	public class RecursiveOctreeTest {
		private IOctree<TestClass> octree;

		[SetUp]
		public void SetUp() {
			octree = new RecusiveOctree<TestClass>(64, 4);
		}

		[Test]
		public void InsertCenterTest() {
			octree.Insert(new TestClass(AABB.FromCenterAndDimension(Vector3d.Zero, Vector3d.One)));

			Assert.AreEqual(1, octree.Items().Count);

			foreach (var child in octree.Children()) {
				Assert.AreEqual(1, child.Items().Count);
			}
		}

		[Test]
		public void InsertFirstOctandTest() {
			octree.Insert(new TestClass(AABB.FromCenterAndDimension(new Vector3d(30), Vector3d.One)));

			Assert.AreEqual(1, octree.Items().Count);

			Assert.AreEqual(null, octree.Children()[0]);
			Assert.AreEqual(null, octree.Children()[1]);
			Assert.AreEqual(null, octree.Children()[2]);
			Assert.AreEqual(null, octree.Children()[3]);
			Assert.AreEqual(null, octree.Children()[4]);
			Assert.AreEqual(null, octree.Children()[5]);
			Assert.AreEqual(null, octree.Children()[6]);
			Assert.AreNotEqual(null, octree.Children()[7]);
		}

		[Test]
		public void Insert2Test() {
			var item = new TestClass(AABB.FromCenterAndDimension(new Vector3d(30), Vector3d.One));
			octree.Insert(item);
			octree.Insert(item);

			Assert.AreEqual(1, octree.Items().Count);

			Assert.AreEqual(null, octree.Children()[0]);
			Assert.AreEqual(null, octree.Children()[1]);
			Assert.AreEqual(null, octree.Children()[2]);
			Assert.AreEqual(null, octree.Children()[3]);
			Assert.AreEqual(null, octree.Children()[4]);
			Assert.AreEqual(null, octree.Children()[5]);
			Assert.AreEqual(null, octree.Children()[6]);
			Assert.AreNotEqual(null, octree.Children()[7]);
		}

		[Test]
		public void DepthTest() {
			var item = new TestClass(AABB.FromCenterAndDimension(new Vector3d(-31), Vector3d.One));
			octree.Insert(item);

			Assert.AreEqual(null, octree.Children()[1]);
			Assert.AreEqual(null, octree.Children()[2]);
			Assert.AreEqual(null, octree.Children()[3]);
			Assert.AreEqual(null, octree.Children()[4]);
			Assert.AreEqual(null, octree.Children()[5]);
			Assert.AreEqual(null, octree.Children()[6]);
			Assert.AreEqual(null, octree.Children()[7]);
			
			var counter = 0;
			var section = octree;
			while (!section.IsLeaf()) {
				counter++;
				section = section.Children()[0];
			}

			Assert.AreEqual(4, counter);
		}

		[Test]
		public void RemoveTest() {
			var item = new TestClass(AABB.FromCenterAndDimension(Vector3d.Zero, new Vector3d(32)));
			octree.Insert(item);
			Assert.AreEqual(1, octree.Items().Count);
			
			octree.Remove(item);
			Assert.AreEqual(0, octree.Items().Count);

			for (var i = 0; i < 8; i++) {
				Assert.AreEqual(null, octree.Children()[i]);
			}
		}
	}

	class TestClass : IOctreeItem<TestClass> {
		private static int counter;
		private readonly int hash = counter++;
		
		private readonly AABB aabb;
		public TestClass(AABB bb) => aabb = bb;
		
		public ICollection<IOctree<IOctreeItem<TestClass>>> GetParents() {
			throw new System.NotImplementedException();
		}

		public AABB GetAABB() => aabb;

		public override int GetHashCode() => hash;
	}
}