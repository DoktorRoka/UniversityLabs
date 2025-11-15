#include "ExpressionEvaluator.h"
#include <algorithm>
#include <fstream>

ExpressionEvaluator::ExpressionEvaluator()
    : operandCount(20), operands(new double[20])
{
    std::fill(operands, operands + operandCount, 0.0);
}

ExpressionEvaluator::ExpressionEvaluator(size_t n)
    : operandCount(n), operands(new double[n])
{
    std::fill(operands, operands + operandCount, 0.0);
}

ExpressionEvaluator::~ExpressionEvaluator()
{
    delete[] operands;
}

void ExpressionEvaluator::setOperand(size_t pos, double value)
{
    if (pos < operandCount) {
        operands[pos] = value;
    }
}

void ExpressionEvaluator::setOperands(double ops[], size_t n)
{
    size_t count = std::min(operandCount, n);
    for (size_t i = 0; i < count; ++i) {
        operands[i] = ops[i];
    }
}

void ExpressionEvaluator::printOperand(std::ostream& os, double value) const
{
    if (value < 0) {
        os << "(" << value << ")";
    }
    else {
        os << value;
    }
}

void ExpressionEvaluator::logToScreen() const
{
    printExpression(std::cout);
    std::cout << std::endl;
}

void ExpressionEvaluator::logToFile(const std::string& filename) const
{
    std::ofstream out(filename, std::ios_base::app | std::ios_base::out);
    if (out.is_open()) {
        printExpression(out);
        out << std::endl;
    }
}
