using System;
using Mtype = System.Double;

namespace D3 {

    class point : ICloneable {
        const double MinNormal = 2.2250738585072014E-308d;
        public Mtype x {
            get;
        }
        public Mtype y {
            get;
        }
        public Mtype z {
            get;
        }

        /// Create a new point in 3d
        public point(Mtype mx, Mtype my, Mtype mz) {
            this.x = mx;
            this.y = my;
            this.z = mz;
        }

        /// Calculate distance between two points
        public Mtype distance(point rhs) {
            return Math.Sqrt(Math.Pow(rhs.x-this.x, 2) + Math.Pow(rhs.y-this.y, 2) + Math.Pow(rhs.z-this.z, 2));
        }

        /// Check if two points are identical
        public bool equal(point rhs) {
            return (this.x == rhs.x && this.y == rhs.y && this.z == rhs.z);
        }

        /// Check if two points are neal equal using an epsilon
        public bool near_equal(point rhs, Mtype epsilon) {
            if (this.equal(rhs)) {
                return true;
            }
            point abs_diff;
            {
                point abs_this = new point(Math.Abs(this.x), Math.Abs(this.y), Math.Abs(this.z));
                point abs_rhs  = new point(Math.Abs(rhs.x), Math.Abs(rhs.y), Math.Abs(rhs.z));
                abs_diff       = new point(Math.Abs(this.x-rhs.x), Math.Abs(this.y-rhs.y), Math.Abs(this.z-rhs.z));
            }
            Mtype tol = MinNormal * epsilon;
            return (abs_diff.x < tol && abs_diff.y < tol && abs_diff.z < tol);
        }

        /// Check if 3 points are linear
        public bool is_linear(point rhs, point rhs1) {
            if (this.equal(rhs) && this.equal(rhs1))
                return true;

            Mtype a = Math.Sqrt(Math.Pow(rhs.x-this.x, 2) + Math.Pow(rhs.y-this.y, 2) + Math.Pow(rhs.z-this.z, 2));
            Mtype b = Math.Sqrt(Math.Pow(rhs1.x-this.x, 2) + Math.Pow(rhs1.y-this.y, 2) + Math.Pow(rhs1.z-this.z, 2));
            Mtype c = Math.Sqrt(Math.Pow(rhs1.x-rhs.x, 2) + Math.Pow(rhs1.y-rhs.y, 2) + Math.Pow(rhs1.z-rhs.z, 2));

            return !(a + b > c && a + c > b && b + c > a);
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
                    + Math.Pow(this.coords.Item2.y-this.coords.Item1.y, 2)
                    + Math.Pow(this.coords.Item2.z-this.coords.Item1.z, 2));
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
            // calculating linear equation
            Mtype l = this.coords.Item1.x-this.coords.Item2.x;
            Mtype m = this.coords.Item1.y-this.coords.Item2.y;
            Mtype n = this.coords.Item1.z-this.coords.Item2.z;

            Mtype l1 = rhs.coords.Item1.x-rhs.coords.Item2.x;
            Mtype m1 = rhs.coords.Item1.y-rhs.coords.Item1.y;
            Mtype n1 = rhs.coords.Item1.z-rhs.coords.Item1.z;
            if (l > l1) {
                Mtype abs_l = Math.Abs(l/l1);
                if (abs_l == Math.Abs(m/m1) && abs_l == Math.Abs(n/n1)) {
                    return true;
                }
            } else {
                Mtype abs_l = Math.Abs(l1/l);
                if (abs_l == Math.Abs(m1/m) && abs_l == Math.Abs(n1/n)) {
                    return true;
                }
            }
            return false;
        }

        /// Check if two lines intersect
        public bool is_intersecting(line rhs) {
            // https://stackoverflow.com/questions/55220355/how-to-detect-whether-two-segmentin-3d-spaceintersect
            // Calculating parametric equation
            Mtype A, B, C, D, E, F;
            A = this.coords.Item2.x - this.coords.Item1.x;
            B = rhs.coords.Item1.x - rhs.coords.Item2.x;
            C = rhs.coords.Item1.x - this.coords.Item1.x;
            D = this.coords.Item2.y - this.coords.Item1.y;
            E = rhs.coords.Item1.y - rhs.coords.Item2.y;
            F = rhs.coords.Item1.y - this.coords.Item1.y;

            Mtype t = (C*E-F*B)/(E*A-B*D);
            Mtype s = (D*C-A*F)/(D*B-A*E);
            return ((t*(this.coords.Item2.z-this.coords.Item1.z)+s*(rhs.coords.Item1.z-rhs.coords.Item2.z)) == rhs.coords.Item1.z-this.coords.Item2.z);
        }

        /// Check if two lines are perpendicular
        public bool is_perpenduclar(line rhs) {
            //TODO
            return false;
        }

        /// Check if a point lies in line
        public bool is_point_inside(point p) {
            // Calculate linear equation and replace x by points coords
            // if l0 == l1 == l2 then point lies in line
            Mtype l = this.coords.Item1.x-this.coords.Item2.x;
            Mtype m = this.coords.Item1.y-this.coords.Item2.y;
            Mtype n = this.coords.Item1.z-this.coords.Item2.z;
            Mtype l0 = (p.x-this.coords.Item2.x/l);
            return (l0 == (p.y-this.coords.Item2.y/l) && l0 == (p.z-this.coords.Item2.z));
        }
    }
}
