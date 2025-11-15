#include "Divisor.h"

double Divisor::calculate() const
{
    if (operandCount == 0) return 0.0;

    // Если хотя бы один операнд 0 – результат 0
    for (size_t i = 0; i < operandCount; ++i) {
        if (operands[i] == 0.0) return 0.0;
    }

    double result = operands[0];
    for (size_t i = 1; i < operandCount; ++i) {
        result /= operands[i];
    }
    return result;
}

void Divisor::printExpression(std::ostream& os) const
{
    if (operandCount == 0) return;

    printOperand(os, operands[0]);
    for (size_t i = 1; i < operandCount; ++i) {
        os << " / ";
        printOperand(os, operands[i]);
    }
}
