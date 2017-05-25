
namespace CompetetiveProgramming.Physics {
    public class Collision {
        public Disk Disk1;
        public Disk Disk2;
        public float TimeTo;
        public Collision(Disk d1, Disk d2, float time) {
            Disk1 = d1;
            Disk2 = d2;
            TimeTo = time;
        }
    }
}
