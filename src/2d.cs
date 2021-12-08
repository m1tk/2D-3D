using System;
using Mtype = System.Double;

namespace D2 {

    public class point : ICloneable {
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

    public class line {
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
            Mtype det, gamma, lambda;
            Mtype a = this.coords.Item1.x;
            Mtype b = this.coords.Item1.y;
            Mtype c = this.coords.Item2.x;
            Mtype d = this.coords.Item2.y;

            Mtype p = rhs.coords.Item1.x;
            Mtype q = rhs.coords.Item1.y;
            Mtype r = rhs.coords.Item2.x;
            Mtype s = rhs.coords.Item2.y;

            det = (c - a) * (s - q) - (r - p) * (d - b);
            if (det == 0.0) {
                return false;
            } else {
                lambda = ((s - q) * (r - a) + (p - r) * (s - b)) / det;
                gamma = ((b - d) * (r - a) + (c - a) * (s - b)) / det;
                return (0 < lambda && lambda < 1) && (0 < gamma && gamma < 1);
            }
        }

        /// Check if line intersecting with circle
        public bool is_intersecting(circle c) {
            Mtype a = this.coords.Item1.y-this.coords.Item2.y;
            Mtype b = this.coords.Item1.x-this.coords.Item2.x;
            Mtype d = this.coords.Item1.x*this.coords.Item2.y
                - this.coords.Item2.x*this.coords.Item1.y;

            return (c.ray > Math.Abs(a*c.r.x + b*c.r.y +d)/ Math.Sqrt(a*a + b*b));
        }


        /// Check if line intersecting with ellipse
        public bool is_intersecting(ellipse e) {
            return e.is_intersecting(this);
        }

        /// Check if two lines are perpendicular
        public bool is_perpendicular(line rhs) {
            Mtype x1 = this.coords.Item1.x;
            Mtype y1 = this.coords.Item1.y;
            Mtype x2 = this.coords.Item2.x;
            Mtype y2 = this.coords.Item2.y;

            Mtype x3 = rhs.coords.Item1.x;
            Mtype y3 = rhs.coords.Item1.y;
            Mtype x4 = rhs.coords.Item2.x;
            Mtype y4 = rhs.coords.Item2.y;

            Mtype m1, m2;

            // Both lines have infinite slope
            if (x2 - x1 == 0 && x4 - x3 == 0)
                return false;

            // Only line 1 has infinite slope
            else if (x2 - x1 == 0) {

                m2 = (y4 - y3) / (x4 - x3);

                if (m2 == 0)
                    return true;
                else
                    return false;
            }

            // Only line 2 has infinite slope
            else if (x4 - x3 == 0) {

                m1 = (y2 - y1) / (x2 - x1);

                if (m1 == 0)
                    return true;
                else
                    return false;
            }

            else {
                // Find slopes of the lines
                m1 = (y2 - y1) / (x2 - x1);
                m2 = (y4 - y3) / (x4 - x3);

                // Check if their product is -1
                if (m1 * m2 == -1)
                    return true;
                else
                    return false;
            }
        }

        /// Check if a point lies in line
        public bool is_point_inside(point p) {
            return p.is_linear(this.coords.Item1, this.coords.Item2);
        }
    }

    /// Create a circle
    public class circle {
        public point r {
            get;
        }
        public Mtype ray {
            get;
        }

        /// Initialize a new cirle
        public circle(point r, Mtype mray) {
            this.r   = (point)r.Clone();
            this.ray = mray;
        }

        /// Get circle permiter
        public Mtype perimeter() {
            return 2*Math.PI*this.ray;
        }

        /// Get circle surface
        public Mtype surface() {
            return 4 * Math.PI * (this.ray*this.ray);
        }

        /// Check if point is inside
        public bool is_point_inside(point p) {
            return (p.distance(this.r) <= this.ray);
        }

        /// Circle interstion with line
        public bool is_intersecting(line l) {
            return l.is_intersecting(this);
        }

        /// Check if two circles intersecting
        public bool is_intersecting(circle rhs) {
            Mtype dist = (this.r.x - rhs.r.x)*(this.r.x-rhs.r.x)
                    + (this.r.y-rhs.r.y)*(this.r.y-rhs.r.y);
            Mtype radsum = (this.ray+rhs.ray)*(this.ray+rhs.ray);

            return (dist <= radsum);
        }

        /// Check if circle and ellipse intersecting
        public bool is_intersecting(ellipse e) {
            return e.is_intersecting(this);
        }
    }

    public class ellipse {
        // center of ellipse
        public Mtype h {
            get;
        }
        public Mtype k {
            get;
        }

        // Semi major axis
        public Mtype a {
            get;
        }
        public Mtype b {
            get;
        }

        /// Create new ellipse object
        public ellipse(Mtype mh, Mtype mk, Mtype ma, Mtype mb) {
            this.h = mh;
            this.k = mk;
            this.a = ma;
            this.b = mb;
        }

        /// Get ellipse perimeter
        public Mtype perimeter() {
            return 2*Math.PI*Math.Sqrt((this.a*this.a + this.b*this.b)/2);
        }

        /// Get ellipse surface
        public Mtype surface() {
            return Math.PI*this.a*this.b;
        }

        /// Check if point is inside ellipse
        public bool is_point_inside(point p) {
            // (p.x-h)^2/a^2+(p.y-k)^2/b^2 <= 1
            return (
                    (Math.Pow(p.x-h, 2)/(a*a))
                    + (Math.Pow(p.y-k, 2)/(b*b))
                    ) <= 1;
        }

        /// Check if ellipse intersect with line
        public bool is_intersecting(line l) {
            Mtype xe = this.h;
            Mtype ye = this.k;
            Mtype rex = this.a;
            Mtype rey = this.b;

            Mtype x1 = l.coords.Item1.x;
            Mtype y1 = l.coords.Item1.y;
            Mtype x2 = l.coords.Item2.x;
            Mtype y2 = l.coords.Item2.y;

            x1 -= xe;
            x2 -= xe;
            y1 -= ye;
            y2 -= ye;

            Mtype A = Math.Pow(x2 - x1, 2) / rex / rex + Math.Pow(y2 - y1, 2) / rey / rey;
            Mtype B = 2 * x1 * (x2 - x1) / rex / rex + 2 * y1 * (y2 - y1) / rey / rey;
            Mtype C = x1 * x1 / rex / rex + y1 * y1 / rey / rey - 1;
            Mtype D = B * B - 4 * A * C;
            if (D == 0) {
                Mtype t = -B / 2 / A;
                return t >= 0 && t <= 1;
            } else if (D > 0) {
                Mtype sqrt = Math.Sqrt(D);
                Mtype t1 = (-B + sqrt) / 2 / A;
                Mtype t2 = (-B - sqrt) / 2 / A;
                return (t1 >= 0 && t1 <= 1) || (t2 >= 0 && t2 <= 1);
            } else {
                return false;
            }
        }

        /// Check if ellpise intersect with ellipse
        public bool is_intersecting(ellipse rhs) {
            Mtype x0 = this.h;
            Mtype y0 = this.k;
            Mtype x1 = rhs.h;
            Mtype y1 = rhs.k;

            Mtype h0 = this.a;
            Mtype w0 = this.b;
            Mtype h1 = rhs.a;
            Mtype w1 = rhs.b;

            Mtype x = Math.Abs(x1 - x0) * h1;
            Mtype y = Math.Abs(y1 - y0) * w1;
            w0 *= h1;
            h0 *= w1;
            Mtype r = w1 * h1;

            if (x * x + (h0 - y) * (h0 - y) <= r * r || (w0 - x) * (w0 - x) + y * y <= r * r || x * h0 + y * w0 <= w0 * h0
                || ((x * h0 + y * w0 - w0 * h0) * (x * h0 + y * w0 - w0 * h0) <= r * r * (w0 * w0 + h0 * h0) && x * w0 - y * h0 >= -h0 * h0 && x * w0 - y * h0 <= w0 * w0))
            {
                return true;
            }
            return false;
        }

        /// Check if ellipse intersect with circle
        public bool is_intersecting(circle c) {
            Mtype x0 = this.h;
            Mtype y0 = this.k;
            Mtype h  = this.a;
            Mtype w  = this.b;

            Mtype x1 = c.r.x;
            Mtype y1 = c.r.y;
            Mtype r  = c.ray;

            var x = Math.Abs(x1 - x0);
            var y = Math.Abs(y1 - y0);

            if (x * x + (h - y) * (h - y) <= r * r || (w - x) * (w - x) + y * y <= r * r || x * h + y * w <= w * h
                || ((x * h + y * w - w * h) * (x * h + y * w - w * h) <= r * r * (w * w + h * h) && x * w - y * h >= -h * h && x * w - y * h <= w * w))
            {
                return true;
            }
            return false;
        }
    }
}
