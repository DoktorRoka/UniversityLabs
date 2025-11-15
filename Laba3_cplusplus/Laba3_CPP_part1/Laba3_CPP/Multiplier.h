#pragma once
#include "ExpressionEvaluator.h"

class Multiplier : public ExpressionEvaluator {
public:
    using ExpressionEvaluator::ExpressionEvaluator;

    double calculate() const override;

protected:
    void printExpression(std::ostream& os) const override;
};
