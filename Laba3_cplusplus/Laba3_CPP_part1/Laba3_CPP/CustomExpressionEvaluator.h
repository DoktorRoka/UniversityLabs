#pragma once
#include "ExpressionEvaluator.h"
#include "IShuffle.h"

class CustomExpressionEvaluator : public ExpressionEvaluator, public IShuffle {
public:
    using ExpressionEvaluator::ExpressionEvaluator;

    double calculate() const override;

protected:
    void printExpression(std::ostream& os) const override;

public:
    void logToScreen() const override;
    void logToFile(const std::string& filename) const override;

    void shuffle() override;
    void shuffle(size_t i, size_t j) override;

private:
    bool hasFractionalPart(double x) const;
};
