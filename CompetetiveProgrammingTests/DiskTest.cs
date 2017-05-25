using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CompetetiveProgramming.Physics;
using CompetetiveProgramming.Geometry;

namespace CompetetiveProgrammingTests {
    [TestClass]
    public class DiskTest {
	private Disk disk = new Disk(new Vector(2, 1), new Vector(2, 3), 5);

	[TestMethod]
	public void testMove() {
		Assert.AreEqual(new Disk(new Vector(4, 4), new Vector(2, 3), 5), disk.Move());
        Assert.AreEqual(new Disk(new Vector(2, 1), new Vector(2, 3), 5), disk);
	}

	[TestMethod]
	public void testAccelerate() {
		Assert.AreEqual(new Disk(new Vector(2, 1), new Vector(3, 5), 5), disk.Accelerate(new Vector(1, 2)));
        Assert.AreEqual(new Disk(new Vector(2, 1), new Vector(4, 6), 5), disk.Accelerate(2));
        Assert.AreEqual(new Disk(new Vector(2, 1), new Vector(2, 3), 5), disk);
	}
	
	[TestMethod]
	public void testCollisionDetection(){
		Disk oppositeMoves = new Disk(new Vector(-2, -5), new Vector(-2, -3), 1);
		Assert.IsFalse(disk.WillCollide(oppositeMoves));
		Disk frontCollision = new Disk(new Vector(6, 7), new Vector(-2, -3), 1);
        Assert.IsTrue(disk.WillCollide(frontCollision));
		Disk alreadyCollide = new Disk(new Vector(2, 1), new Vector(-2, -3), 1);
        Assert.IsTrue(disk.WillCollide(alreadyCollide));
		Disk noRelativeMovement = new Disk(new Vector(10, 10), new Vector(2, 3), 2);
        Assert.IsFalse(disk.WillCollide(noRelativeMovement));
		Disk goingRight = new Disk(new Vector(0,0), new Vector(10, 0), 5);
		Disk radiusTouch = new Disk(new Vector(2, 6), new Vector(0, 0), 1);
        Assert.IsTrue(goingRight.WillCollide(radiusTouch));
		Disk radiusNotTouch = new Disk(new Vector(2, 6), new Vector(0, 0), 0.5);
        Assert.IsFalse(goingRight.WillCollide(radiusNotTouch));
	}

    [TestMethod]
	public void collisionTime(){
		Disk oppositeMoves = new Disk(new Vector(-2, -5), new Vector(-2, -3), 1);
        Assert.AreEqual(double.MaxValue, disk.CollisionTime(oppositeMoves), 0.01);
		Disk goingRight = new Disk(new Vector(0,0), new Vector(10, 0), 5);
		Disk radiusTouch = new Disk(new Vector(2, 6), new Vector(0, 0), 1);
        Assert.AreEqual(0.2, goingRight.CollisionTime(radiusTouch), 0.01);
        Assert.AreEqual(2, goingRight.CollisionTime(new Disk(new Vector(26, 0), new Vector(0, 0), 1)), 0.01);
        Assert.AreEqual(0, goingRight.CollisionTime(goingRight), 0.01);
        Assert.AreEqual(0, goingRight.CollisionTime(new Disk(new Vector(1, 1), new Vector(-10, 0), 5)), 0.01);
	}

}
}
