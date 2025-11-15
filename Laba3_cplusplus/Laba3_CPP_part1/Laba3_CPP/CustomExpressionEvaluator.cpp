#include "CustomExpressionEvaluator.h"
#include <algorithm>
#include <fstream>
#include <cmath>

double CustomExpressionEvaluator::calculate() const
{
    if (operandCount == 0) return 0.0;

    double result = operands[0]; // x1
    size_t i = 1;

    for (; i + 1 < operandCount; i += 2) {
        result += operands[i] * operands[i + 1];  // + x2*x3, + x4*x5 ...
    }

    if (i < operandCount) {   // если остался лишний – просто прибавим
        result += operands[i];
    }

    return result;
}

void CustomExpressionEvaluator::printExpression(std::ostream& os) const
{
    if (operandCount == 0) return;

    printOperand(os, operands[0]);
    size_t i = 1;

    for (; i + 1 < operandCount; i += 2) {
        os << " + ";
        printOperand(os, operands[i]);
        os << "*";
        printOperand(os, operands[i + 1]);
    }

    if (i < operandCount) {
        os << " + ";
        printOperand(os, operands[i]);
    }
}

void CustomExpressionEvaluator::logToScreen() const
{
    printExpression(std::cout);
    std::cout << " < Total " << operandCount << " >" << std::endl;
}

void CustomExpressionEvaluator::logToFile(const std::string& filename) const
{
    std::ofstream log(filename, std::ios_base::app | std::ios_base::out);
    if (log.is_open()) {
        printExpression(log);
        log << " < Total " << operandCount << " >" << std::endl;
    }
}

void CustomExpressionEvaluator::shuffle()
{
    std::sort(operands, operands + operandCount);
}

bool CustomExpressionEvaluator::hasFractionalPart(double x) const
{
    double intPart;
    return std::modf(x, &intPart) != 0.0;
}

void CustomExpressionEvaluator::shuffle(size_t i, size_t j)
{
    if (i >= operandCount || j >= operandCount) return;

    if (hasFractionalPart(operands[i]) || hasFractionalPart(operands[j])) {
        std::swap(operands[i], operands[j]);
    }
}
