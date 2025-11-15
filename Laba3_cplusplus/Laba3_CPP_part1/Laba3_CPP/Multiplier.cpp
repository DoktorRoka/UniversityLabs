#include "Multiplier.h"

double Multiplier::calculate() const
{
    if (operandCount == 0) return 0.0;

    double result = 1.0;
    for (size_t i = 0; i < operandCount; ++i) {
        result *= operands[i];
    }
    return result;
}

void Multiplier::printExpression(std::ostream& os) const
{
    if (operandCount == 0) return;

    printOperand(os, operands[0]);
    for (size_t i = 1; i < operandCount; ++i) {
        os << " * ";
        printOperand(os, operands[i]);
    }
}
