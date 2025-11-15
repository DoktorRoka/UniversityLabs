#pragma once
#include "ExpressionEvaluator.h"

class Summator : public ExpressionEvaluator {
public:
    using ExpressionEvaluator::ExpressionEvaluator;

    double calculate() const override;

protected:
    void printExpression(std::ostream& os) const override;
};
