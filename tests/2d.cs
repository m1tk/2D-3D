using NUnit.Framework;
using D2;

namespace tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void point_equal()
    {
        point p0 = new point(1.0, 2.0);
        point p1 = new point(1.0, 2.0);
        point p2 = new point(1.0, 3.0);
        Assert.IsTrue(p0.equal(p1) && !p0.equal(p2), "point equal not working");
    }

    [Test]
    public void point_distance()
    {
        point p0 = new point(1.0, 2.0);
        point p1 = new point(1.0, 2.0);
        point p2 = new point(2.0, 2.0);
        Assert.IsTrue(p0.distance(p1) == 0.0 && p0.distance(p2) == 1.0, "point distance not working");
    }

    [Test]
    public void point_is_linear()
    {
        point p0 = new point(1.0, 1.0);
        point p1 = new point(1.0, 2.0);
        point p2 = new point(1.0, 3.0);
        point p3 = new point(2.0, 2.0);
        Assert.IsTrue(p0.is_linear(p1, p2) && !p0.is_linear(p1, p3), "point is_linear not working");
    }

    [Test]
    public void line_equal()
    {
        line l0 = new line(new point(1.0, 2.0), new point(3.0, 1.0));
        line l1 = new line(new point(3.0, 1.0), new point(1.0, 2.0));
        line l2 = new line(new point(1.0, 1.0), new point(1.0, 3.0));
        Assert.IsTrue(l0.equal(l1) && !l0.equal(l2), "line equal not working");
    }

    [Test]
    public void line_distance()
    {
        line l0 = new line(new point(1.0, 2.0), new point(2.0, 2.0));
        line l1 = new line(new point(1.0, 1.0), new point(3.0, 1.0));
        Assert.IsTrue(l0.distance() == 1.0 && l1.distance() == 2.0, "line distance not working");
    }

    [Test]
    public void line_is_parralel()
    {
        line l0 = new line(new point(1.0, 2.0), new point(2.0, 2.0));
        line l1 = new line(new point(1.0, 1.0), new point(3.0, 1.0));
        line l2 = new line(new point(1.0, 3.0), new point(3.0, 3.0));
        line l3 = new line(new point(1.0, 3.0), new point(3.0, 0.0));
        Assert.IsTrue(l0.is_parralel(l1) && l0.is_parralel(l2) && !l0.is_parralel(l3), "line is_parralel not working");
    }

    [Test]
    public void line_is_perpendicular()
    {
        line l0 = new line(new point(0.0, 0.0), new point(0.0, 10.0));
        line l1 = new line(new point(1.0, 1.0), new point(3.0, 1.0));
        line l2 = new line(new point(1.0, 3.0), new point(3.0, 1.0));
        line l3 = new line(new point(-1.5, 1.0), new point(4.5, 1.0));
        Assert.IsTrue(l0.is_perpendicular(l1) && !l0.is_perpendicular(l2) && l0.is_perpendicular(l3), "line is_perpendicular not working");
    }

    [Test]
    public void circle_is_point_inside()
    {
        circle c = new circle(new point(0.0, 0.0), 2.0);
        Assert.IsTrue(c.is_point_inside(new point(1.0, 1.0)) && c.is_point_inside(new point(0.0, 2.0)) && !c.is_point_inside(new point(3.0, 0.0)),
                "circle is_point_inside not working");
    }

    [Test]
    public void ellipse_is_point_inside()
    {
        ellipse c = new ellipse(0.0, 0.0, 2.0, 2.0);
        Assert.IsTrue(c.is_point_inside(new point(1.0, 1.0)) && c.is_point_inside(new point(0.0, 2.0)) && !c.is_point_inside(new point(3.0, 0.0)),
                "ellipse is_point_inside not working");
    }

    [Test]
    public void line_line_intersecting()
    {
        line l0 = new line(new point(0.0, 0.0), new point(0.0, 10.0));
        line l1 = new line(new point(-1.0, 1.0), new point(3.0, 1.0));
        line l2 = new line(new point(5.0, 10.0), new point(10.0, 1.0));
        Assert.IsTrue(l0.is_intersecting(l1) && !l0.is_intersecting(l2),
                "line line intersection not working");
    }

    [Test]
    public void line_circle_intersecting()
    {
        line l0  = new line(new point(0.0, 0.0), new point(0.0, 10.0));
        line l2  = new line(new point(5.0, 10.0), new point(10.0, 1.0));
        circle c = new circle(new point(0.0, 0.0), 2);

        Assert.IsTrue(c.is_intersecting(l0) && !c.is_intersecting(l2),
                "line circle intersection not working");
    }

    [Test]
    public void line_ellipse_intersecting()
    {
        line l0   = new line(new point(0.0, 0.0), new point(0.0, 10.0));
        line l2   = new line(new point(5.0, 10.0), new point(10.0, 1.0));
        ellipse c = new ellipse(0.0, 0.0, 2, 2);

        Assert.IsTrue(c.is_intersecting(l0) && !c.is_intersecting(l2),
                "line ellipse intersection not working");
    }

    [Test]
    public void circle_circle_intersecting()
    {
        circle c0 = new circle(new point(0.0, 0.0), 2);
        circle c1 = new circle(new point(3.0, 0.0), 2);
        circle c2 = new circle(new point(5.0, 0.0), 2);

        Assert.IsTrue(c0.is_intersecting(c1) && !c0.is_intersecting(c2),
                "circle circle intersection not working");
    }

    [Test]
    public void ellipse_ellipse_intersecting()
    {
        ellipse c0 = new ellipse(0.0, 0.0, 2, 2);
        ellipse c1 = new ellipse(3.0, 0.0, 2, 2);
        ellipse c2 = new ellipse(5.0, 0.0, 2, 2);

        Assert.IsTrue(c0.is_intersecting(c1) && !c0.is_intersecting(c2),
                "ellipse ellipse intersection not working");
    }

    [Test]
    public void ellipse_circle_intersecting()
    {
        ellipse c0 = new ellipse(0.0, 0.0, 2, 2);
        circle c1 = new circle(new point(3.0, 0.0), 2);
        circle c2 = new circle(new point(5.0, 0.0), 2);

        Assert.IsTrue(c0.is_intersecting(c1) && !c0.is_intersecting(c2),
                "ellipse circle intersection not working");
    }
}
