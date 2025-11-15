#include <iostream>
#include <string>
#include "Divisor.h"
#include "CustomExpressionEvaluator.h"
#include "Multiplier.h"

int main()
{
    ExpressionEvaluator* evaluators[3];

    // 1) Divisor: 150, -3, 10, -2.5
    evaluators[0] = new Divisor(4);
    evaluators[0]->setOperand(0, 150);
    evaluators[0]->setOperand(1, -3);
    evaluators[0]->setOperand(2, 10);
    evaluators[0]->setOperand(3, -2.5);

    // 2) CustomExpressionEvaluator: 5, 16, -3, 10, 12
    evaluators[1] = new CustomExpressionEvaluator(5);
    double customOps[] = { 5, 16, -3, 10, 12 };
    evaluators[1]->setOperands(customOps, 5);

    // 3) Multiplier: 1.5, 4, -2.5, -8, -15
    evaluators[2] = new Multiplier(5);
    double multOps[] = { 1.5, 4, -2.5, -8, -15 };
    evaluators[2]->setOperands(multOps, 5);

    const std::string logFile = "Lab3.log";

    std::cout << "------Начальные выражения-------\n";

    for (int i = 0; i < 3; ++i) {
        evaluators[i]->logToScreen();
        double res = evaluators[i]->calculate();
        std::cout << "< Result " << res << " >\n\n";

        evaluators[i]->logToFile(logFile);
    }

    std::cout << "-----После шафла------\n";

    for (int i = 0; i < 3; ++i) {
        IShuffle* sh = dynamic_cast<IShuffle*>(evaluators[i]);
        if (sh) {
            sh->shuffle();
            if (evaluators[i]->getOperandsCount() >= 2) {
                sh->shuffle(0, 1);
            }
            evaluators[i]->logToScreen();
            double res = evaluators[i]->calculate();
            std::cout << "< Result " << res << " >\n\n";
        }
    }

    for (int i = 0; i < 3; ++i) {
        delete evaluators[i];
    }

    return 0;
}
