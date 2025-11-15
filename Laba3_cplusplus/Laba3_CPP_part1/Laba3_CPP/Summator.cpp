#include "Summator.h"

double Summator::calculate() const
{
    double result = 0.0;
    for (size_t i = 0; i < operandCount; ++i) {
        result += operands[i];
    }
    return result;
}

void Summator::printExpression(std::ostream& os) const
{
    if (operandCount == 0) return;

    printOperand(os, operands[0]);
    for (size_t i = 1; i < operandCount; ++i) {
        os << " + ";
        printOperand(os, operands[i]);
    }
}
