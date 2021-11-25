using System;
using Mtype = System.Double;

namespace D2 {

    class point : ICloneable {
        const double MinNormal = 2.2250738585072014E-308d;
        public Mtype x {
            get;
        }
        public Mtype y {
            get;
        }

        /// Create a new point in 2d
        public point(Mtype mx, Mtype my) {
            this.x = mx;
            this.y = my;
        }

        /// Calculate distance between two points
        public Mtype distance(point rhs) {
            return Math.Sqrt(Math.Pow(rhs.x-this.x, 2) + Math.Pow(rhs.y-this.y, 2));
        }

        /// Check if two points are identical
        public bool equal(point rhs) {
            return (this.x == rhs.x && this.y == rhs.y);
        }

        /// Check if two points are neal equal using an epsilon
        public bool near_equal(point rhs, Mtype epsilon) {
            if (this.equal(rhs)) {
                return true;
            }
            point abs_diff;
            {
                point abs_this = new point(Math.Abs(this.x), Math.Abs(this.y));
                point abs_rhs  = new point(Math.Abs(rhs.x), Math.Abs(rhs.y));
                abs_diff       = new point(Math.Abs(this.x-rhs.x), Math.Abs(this.y-rhs.y));
            }
            Mtype tol = MinNormal * epsilon;
            return (abs_diff.x < tol && abs_diff.y < tol);
        }

        /// Check if 2d points are linear
        public bool is_linear(point rhs, point rhs1) {
            if (this.equal(rhs) && this.equal(rhs1))
                return true;

            return (((rhs.x - x)*(rhs.y - y) - (rhs1.y - y)*(rhs1.x - x)) == 0 ? true : false);
        }

        public object Clone() {
            return this.MemberwiseClone();
        }

    }

    class line {
        public Tuple<point, point> coords {
            get;
        }

        public line(point a, point b) {
            this.coords = new Tuple<point, point>((point)a.Clone(), (point)b.Clone());
        }


        /// Calculate distance between coords of line
        public Mtype distance() {
            return Math.Sqrt(Math.Pow(this.coords.Item2.x-this.coords.Item1.x, 2)
                    + Math.Pow(this.coords.Item2.y-this.coords.Item1.y, 2));
        }

        /// Check if two lines are identical
        public bool equal(line rhs) {
            if ((this.coords.Item1.equal(rhs.coords.Item1) && this.coords.Item2.equal(rhs.coords.Item2))
            || (this.coords.Item1.equal(rhs.coords.Item2) && this.coords.Item2.equal(rhs.coords.Item1))) {
                return true;
            }
            return false;
        }

        /// Check if two lines are parralel
        public bool is_parralel(line rhs) {
            Mtype l  = (this.coords.Item1.x-this.coords.Item2.x)/(this.coords.Item1.y-this.coords.Item2.y);
            Mtype l1 = (rhs.coords.Item1.x-rhs.coords.Item2.x)/(rhs.coords.Item1.y-rhs.coords.Item2.y);
            return (l == l1);
        }

        /// Check if two lines intersect
        public bool is_intersecting(line rhs) {
            Mtype l  = (this.coords.Item1.x-this.coords.Item2.x)/(this.coords.Item1.y-this.coords.Item2.y);
            Mtype l1 = (rhs.coords.Item1.x-rhs.coords.Item2.x)/(rhs.coords.Item1.y-rhs.coords.Item2.y);
            return (l != l1);
        }
        /// Check if two lines are perpendicular
        public bool is_perpenduclar(line rhs) {
            Mtype l  = (this.coords.Item1.x-this.coords.Item2.x)/(this.coords.Item1.y-this.coords.Item2.y);
            Mtype l1 = (rhs.coords.Item1.x-rhs.coords.Item2.x)/(rhs.coords.Item1.y-rhs.coords.Item2.y);
            return (l*l1 == -1);
        }

        /// Check if a point lies in line
        public bool is_point_inside(point p) {
            return p.is_linear(this.coords.Item1, this.coords.Item2);
        }
    }

    /// Create a circle
    class circle {
        public point r {
            get;
        }
        public Mtype ray {
            get;
        }

        public circle(point r, Mtype mray) {
            this.r   = (point)r.Clone();
            this.ray = mray;
        }

        public Mtype perimeter() {
            return 2*Math.PI*this.ray;
        }

        public Mtype surface() {
            return 4 * Math.PI * (this.ray*this.ray);
        }

        public bool is_point_inside(point p) {
            return (p.distance(this.r) <= this.ray);
        }
    }
}
