#pragma once
#include <cstddef>
#include <string>
#include <iostream>
#include "ILoggable.h"

class ExpressionEvaluator : public ILoggable {
protected:
    size_t operandCount;
    double* operands;

    void printOperand(std::ostream& os, double value) const;
    virtual void printExpression(std::ostream& os) const = 0;

public:
    ExpressionEvaluator();
    explicit ExpressionEvaluator(size_t n);
    virtual ~ExpressionEvaluator();

    void setOperand(size_t pos, double value);
    void setOperands(double ops[], size_t n);

    size_t getOperandsCount() const { return operandCount; }

    virtual double calculate() const = 0;

    void logToScreen() const override;
    void logToFile(const std::string& filename) const override;
};
