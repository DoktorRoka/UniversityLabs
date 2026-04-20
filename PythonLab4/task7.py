import sympy as sp
import matplotlib.pyplot as plt
import numpy as np


def task_derivative_and_integral():
    print("=== Derivative and Integral of f(x) = x^2 * sin(x) ===")
    x = sp.Symbol("x")
    f = x**2 * sp.sin(x)

    f_prime = sp.diff(f, x)
    f_integral = sp.integrate(f, x)

    print(f"f(x) = {f}")
    print(f"f'(x) = {f_prime}")
    print(f"Integral of f(x) dx = {f_integral} + C")

    f_num = sp.lambdify(x, f, "numpy")
    f_prime_num = sp.lambdify(x, f_prime, "numpy")
    f_integral_num = sp.lambdify(x, f_integral, "numpy")

    x_vals = np.linspace(-2 * np.pi, 2 * np.pi, 500)

    fig, axes = plt.subplots(1, 3, figsize=(15, 4))

    axes[0].plot(x_vals, f_num(x_vals), "b-")
    axes[0].set_title(f"f(x) = {f}")
    axes[0].grid(True)

    axes[1].plot(x_vals, f_prime_num(x_vals), "r-")
    axes[1].set_title(f"f'(x) = {f_prime}")
    axes[1].grid(True)

    axes[2].plot(x_vals, f_integral_num(x_vals), "g-")
    axes[2].set_title(f"Integral = {f_integral}")
    axes[2].grid(True)

    plt.tight_layout()
    plt.savefig("task7_derivative_integral.png", dpi=100)
    plt.show()
    print("Plot saved to task7_derivative_integral.png")


def task_solve_nonlinear_equation():
    print("\n=== Solving a nonlinear equation: x^3 - 2x + 1 = 0 ===")
    x = sp.Symbol("x")
    eq = x**3 - 2*x + 1
    solutions = sp.solve(eq, x)
    print(f"Equation: {eq} = 0")
    print(f"Solutions: {solutions}")

    f_num = sp.lambdify(x, eq, "numpy")
    x_vals = np.linspace(-3, 3, 500)

    plt.figure(figsize=(8, 5))
    plt.plot(x_vals, f_num(x_vals), "b-", label=str(eq))
    plt.axhline(y=0, color="k", linewidth=0.5)
    for sol in solutions:
        sol_val = float(sol)
        plt.plot(sol_val, 0, "ro", markersize=8)
        plt.annotate(f"x={sol}", (sol_val, 0), textcoords="offset points",
                     xytext=(5, 10), fontsize=9)
    plt.title(f"Solutions of {eq} = 0")
    plt.legend()
    plt.grid(True)
    plt.savefig("task7_nonlinear_eq.png", dpi=100)
    plt.show()
    print("Plot saved to task7_nonlinear_eq.png")


def task_solve_nonlinear_system():
    print("\n=== Solving a system of nonlinear equations ===")
    x, y = sp.symbols("x y")
    eq1 = x**2 + y**2 - 4
    eq2 = x - y - 1

    solutions = sp.solve([eq1, eq2], [x, y])
    print(f"System:")
    print(f"  {eq1} = 0")
    print(f"  {eq2} = 0")
    print(f"Solutions: {solutions}")

    eq1_num = sp.lambdify((x, y), eq1, "numpy")
    eq2_num = sp.lambdify((x, y), eq2, "numpy")

    x_vals = np.linspace(-3, 3, 400)
    y_vals = np.linspace(-3, 3, 400)
    X, Y = np.meshgrid(x_vals, y_vals)

    plt.figure(figsize=(8, 8))
    plt.contour(X, Y, eq1_num(X, Y), levels=[0], colors=["blue"])
    plt.contour(X, Y, eq2_num(X, Y), levels=[0], colors=["red"])

    for sol in solutions:
        sx, sy = float(sol[0]), float(sol[1])
        plt.plot(sx, sy, "go", markersize=10)
        plt.annotate(f"({sx:.2f}, {sy:.2f})", (sx, sy),
                     textcoords="offset points", xytext=(10, 5), fontsize=9)

    plt.xlabel("x")
    plt.ylabel("y")
    plt.title("System of nonlinear equations")
    plt.legend(["x^2 + y^2 = 4", "x - y = 1", "Solutions"], loc="upper right")
    plt.grid(True)
    plt.axis("equal")
    plt.savefig("task7_nonlinear_system.png", dpi=100)
    plt.show()
    print("Plot saved to task7_nonlinear_system.png")


if __name__ == "__main__":
    task_derivative_and_integral()
    task_solve_nonlinear_equation()
    task_solve_nonlinear_system()
