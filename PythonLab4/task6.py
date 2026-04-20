import numpy as np


def task_1_multiply_matrices():
    print("=== 1) Multiplication of A(3x5) and B(5x2) ===")
    A = np.array([
        [1, 2, 3, 4, 5],
        [6, 7, 8, 9, 10],
        [11, 12, 13, 14, 15]
    ])
    B = np.array([
        [1, 2],
        [3, 4],
        [5, 6],
        [7, 8],
        [9, 10]
    ])
    C = np.dot(A, B)
    print(f"A =\n{A}")
    print(f"B =\n{B}")
    print(f"A @ B =\n{C}")
    return C


def task_2_matrix_vector_multiply():
    print("\n=== 2) Multiplication of matrix(5x3) by 3D vector ===")
    M = np.array([
        [1, 2, 3],
        [4, 5, 6],
        [7, 8, 9],
        [10, 11, 12],
        [13, 14, 15]
    ])
    v = np.array([1, 0, -1])
    result = np.dot(M, v)
    print(f"M =\n{M}")
    print(f"v = {v}")
    print(f"M @ v = {result}")
    return result


def task_3_solve_linear_system():
    print("\n=== 3) Solving a system of linear equations ===")
    A = np.array([
        [2, 1, -1],
        [-3, -1, 2],
        [-2, 1, 2]
    ])
    b = np.array([8, -11, -3])
    x = np.linalg.solve(A, b)
    print(f"System: 2x + y - z = 8; -3x - y + 2z = -11; -2x + y + 2z = -3")
    print(f"Solution: x = {x}")
    print(f"Verification A@x = {np.dot(A, x)} (should be {b})")
    return x


def task_4_determinant():
    print("\n=== 4) Determinant of a matrix ===")
    M = np.array([
        [1, 2, 3],
        [4, 5, 6],
        [7, 8, 10]
    ])
    det = np.linalg.det(M)
    print(f"M =\n{M}")
    print(f"det(M) = {det:.6f}")
    return det


def task_5_inverse_and_transpose():
    print("\n=== 5) Inverse and Transposed matrices ===")
    M = np.array([
        [1, 2, 3],
        [0, 1, 4],
        [5, 6, 0]
    ])
    inv_M = np.linalg.inv(M)
    trans_M = M.T
    print(f"M =\n{M}")
    print(f"Inverse(M) =\n{inv_M}")
    print(f"M @ Inverse(M) =\n{np.dot(M, inv_M)} (should be identity)")
    print(f"Transpose(M) =\n{trans_M}")
    return inv_M, trans_M


def task_6_eigenvalues_determinant():
    print("\n=== 6) Determinant equals product of eigenvalues (5x5) ===")
    np.random.seed(42)
    M = np.random.rand(5, 5)
    M = (M + M.T) / 2

    det_M = np.linalg.det(M)
    eigenvalues = np.linalg.eigvals(M)
    product_eigenvalues = np.prod(eigenvalues)

    print(f"M =\n{M}")
    print(f"Eigenvalues = {eigenvalues}")
    print(f"det(M) = {det_M:.10f}")
    print(f"Product of eigenvalues = {product_eigenvalues:.10f}")
    print(f"Difference = {abs(det_M - product_eigenvalues):.2e} (should be ~0)")
    return det_M, product_eigenvalues


if __name__ == "__main__":
    task_1_multiply_matrices()
    task_2_matrix_vector_multiply()
    task_3_solve_linear_system()
    task_4_determinant()
    task_5_inverse_and_transpose()
    task_6_eigenvalues_determinant()
