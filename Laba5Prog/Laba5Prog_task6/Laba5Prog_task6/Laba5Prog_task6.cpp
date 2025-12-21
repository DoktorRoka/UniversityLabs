// Lab #4 (OOP): Aggregation/Composition, Friends, Exceptions, Adapter (GOF)
// Variant 5: "17 + 4", type: basic, extra rule: split
// Modified: STL collections + robust UTF-8 output for Russian text (no "кракозябры").
//
// IMPORTANT ABOUT ENCODING (Windows):
// - This file contains ONLY ASCII characters inside string literals by using \uXXXX escapes,
//   so it prints correctly even if your editor saves the file in ANSI/Windows-1251.
// - If stdout is not a real console (some IDE output windows), we fallback to English ASCII text.
// - Suits (♥♦♣♠) are also auto-fallback to ASCII (H D C S) in non-console output windows.
//
// Build: g++ -std=c++17 -O2 -Wall -Wextra -pedantic lab4_stl_utf8.cpp -o lab4

#ifdef _WIN32
#ifndef NOMINMAX
#define NOMINMAX
#endif
#include <windows.h>
#endif

#include <algorithm>
#include <array>
#include <deque>
#include <iostream>
#include <limits>
#include <numeric>
#include <random>
#include <sstream>
#include <stdexcept>
#include <string>
#include <utility>
#include <vector>

using std::cout;
using std::cin;

// -------------------- Console helper (Windows) --------------------
namespace console {
#ifdef _WIN32
    inline bool is_real_console_stdout() {
        HANDLE h = GetStdHandle(STD_OUTPUT_HANDLE);
        if (h == INVALID_HANDLE_VALUE || h == nullptr) return false;
        DWORD mode = 0;
        return GetConsoleMode(h, &mode) != 0;
    }
    inline void enable_utf8_if_possible() {
        // If it's a real console: enable UTF-8.
        // If it's IDE output window (pipe): do nothing to avoid mojibake.
        if (is_real_console_stdout()) {
            SetConsoleOutputCP(CP_UTF8);
            SetConsoleCP(CP_UTF8);
        }
    }
#else
    inline bool is_real_console_stdout() { return true; }
    inline void enable_utf8_if_possible() {}
#endif
} // namespace console

// -------------------- Text (RU/EN switch) --------------------
namespace text {
    inline const char* header() {
#ifdef _WIN32
        if (!console::is_real_console_stdout())
            return "BlackJack (lab #4). Variant 5: \"17 + 4\", type: basic, extra rule: split.\n";
#endif
        return u8"BlackJack (\u043b\u0430\u0431\u0430 \u21164). \u0412\u0430\u0440\u0438\u0430\u043d\u0442 5: \"17 + 4\", \u0442\u0438\u043f: \u0431\u0430\u0437\u043e\u0432\u044b\u0439, \u0434\u043e\u043f. \u043f\u0440\u0430\u0432\u0438\u043b\u043e: \u0441\u043f\u043b\u0438\u0442.\n";
    }
    inline const char* adapterDemo() {
#ifdef _WIN32
        if (!console::is_real_console_stdout()) return "Adapter demo (task 2):\n";
#endif
        return u8"\u0414\u0435\u043c\u043e\u043d\u0441\u0442\u0440\u0430\u0446\u0438\u044f \u0430\u0434\u0430\u043f\u0442\u0435\u0440\u0430 (\u0437\u0430\u0434\u0430\u043d\u0438\u0435 2):\n";
    }
    inline const char* betAsk() {
#ifdef _WIN32
        if (!console::is_real_console_stdout()) return "Your bet?\n";
#endif
        return u8"\u0412\u0430\u0448\u0430 \u0441\u0442\u0430\u0432\u043a\u0430?\n";
    }
    inline const char* notEnoughMoney() {
#ifdef _WIN32
        if (!console::is_real_console_stdout()) return "Not enough money. Balance: ";
#endif
        return u8"\u041d\u0435\u0434\u043e\u0441\u0442\u0430\u0442\u043e\u0447\u043d\u043e \u0441\u0440\u0435\u0434\u0441\u0442\u0432. \u0412\u0430\u0448 \u0431\u0430\u043b\u0430\u043d\u0441: ";
    }
    inline const char* playAgain() {
#ifdef _WIN32
        if (!console::is_real_console_stdout()) return "\nPlay again? (1-yes, 0-no): ";
#endif
        return u8"\n\u0421\u044b\u0433\u0440\u0430\u0442\u044c \u0435\u0449\u0435? (1 - \u0434\u0430, 0 - \u043d\u0435\u0442): ";
    }
    inline const char* gameOver() {
#ifdef _WIN32
        if (!console::is_real_console_stdout()) return "\nGame over. Your balance: ";
#endif
        return u8"\n\u0418\u0433\u0440\u0430 \u043e\u043a\u043e\u043d\u0447\u0435\u043d\u0430. \u0412\u0430\u0448 \u0431\u0430\u043b\u0430\u043d\u0441: ";
    }
    inline const char* dealerLabel() {
#ifdef _WIN32
        if (!console::is_real_console_stdout()) return "Dealer: ";
#endif
        return u8"\u0414\u0438\u043b\u0435\u0440: ";
    }
    inline const char* youLabel() {
#ifdef _WIN32
        if (!console::is_real_console_stdout()) return "You: ";
#endif
        return u8"\u0412\u044b: ";
    }
    inline const char* handLabel() {
#ifdef _WIN32
        if (!console::is_real_console_stdout()) return "You (hand ";
#endif
        return u8"\u0412\u044b (\u0440\u0443\u043a\u0430 ";
    }
    inline const char* stopMenu() {
#ifdef _WIN32
        if (!console::is_real_console_stdout()) return "1. Stand\n";
#endif
        return u8"1. \u0425\u0432\u0430\u0442\u0438\u0442\n";
    }
    inline const char* hitMenu() {
#ifdef _WIN32
        if (!console::is_real_console_stdout()) return "2. Hit\n";
#endif
        return u8"2. \u0415\u0449\u0435\n";
    }
    inline const char* splitMenu() {
#ifdef _WIN32
        if (!console::is_real_console_stdout()) return "3. Split?\n";
#endif
        return u8"3. \u0421\u043f\u043b\u0438\u0442?\n";
    }
    inline const char* blackjackMsg() {
#ifdef _WIN32
        if (!console::is_real_console_stdout()) return "BLACKJACK (21 in 2 cards)!\n";
#endif
        return u8"\u0423 \u0432\u0430\u0441 \u0411\u041b\u0415\u041a-\u0414\u0416\u0415\u041a (21 \u0437\u0430 2 \u043a\u0430\u0440\u0442\u044b)!\n";
    }
    inline const char* specialMsg() {
#ifdef _WIN32
        if (!console::is_real_console_stdout()) return "Special BLACKJACK \"17 + 4\"!\n";
#endif
        return u8"\u0423 \u0432\u0430\u0441 \u0441\u043f\u0435\u0446. \u0411\u041b\u0415\u041a-\u0414\u0416\u0415\u041a \"17 + 4\"!\n";
    }
    inline const char* bustMsg() {
#ifdef _WIN32
        if (!console::is_real_console_stdout()) return "Bust! (";
#endif
        return u8"\u041f\u0435\u0440\u0435\u0431\u043e\u0440! (";
    }
    inline const char* dealerTakes() {
#ifdef _WIN32
        if (!console::is_real_console_stdout()) return "Dealer takes a card...\n";
#endif
        return u8"\u0414\u0438\u043b\u0435\u0440 \u0431\u0435\u0440\u0435\u0442 \u043a\u0430\u0440\u0442\u0443...\n";
    }
    inline const char* dealerBust() {
#ifdef _WIN32
        if (!console::is_real_console_stdout()) return "Dealer bust! (";
#endif
        return u8"\u0423 \u0434\u0438\u043b\u0435\u0440\u0430 \u043f\u0435\u0440\u0435\u0431\u043e\u0440! (";
    }
    inline const char* roundResult() {
#ifdef _WIN32
        if (!console::is_real_console_stdout()) return "\n--- Round result ---\n";
#endif
        return u8"\n--- \u0418\u0442\u043e\u0433 \u0440\u0430\u0443\u043d\u0434\u0430 ---\n";
    }
    inline const char* dealerScore() {
#ifdef _WIN32
        if (!console::is_real_console_stdout()) return "Dealer score: ";
#endif
        return u8"\u041e\u0447\u043a\u0438 \u0434\u0438\u043b\u0435\u0440\u0430: ";
    }
    inline const char* playerScorePrefix() {
#ifdef _WIN32
        if (!console::is_real_console_stdout()) return "Player score (hand ";
#endif
        return u8"\u041e\u0447\u043a\u0438 \u0438\u0433\u0440\u043e\u043a\u0430 (\u0440\u0443\u043a\u0430 ";
    }
    inline const char* bustWord() {
#ifdef _WIN32
        if (!console::is_real_console_stdout()) return "bust";
#endif
        return u8"\u043f\u0435\u0440\u0435\u0431\u043e\u0440";
    }
    inline const char* handWord() {
#ifdef _WIN32
        if (!console::is_real_console_stdout()) return "Hand ";
#endif
        return u8"\u0420\u0443\u043a\u0430 ";
    }
    inline const char* winWord() {
#ifdef _WIN32
        if (!console::is_real_console_stdout()) return "win";
#endif
        return u8"\u0432\u044b\u0438\u0433\u0440\u044b\u0448";
    }
    inline const char* loseWord() {
#ifdef _WIN32
        if (!console::is_real_console_stdout()) return "lose";
#endif
        return u8"\u043f\u0440\u043e\u0438\u0433\u0440\u044b\u0448";
    }
    inline const char* pushWord() {
#ifdef _WIN32
        if (!console::is_real_console_stdout()) return "push";
#endif
        return u8"\u0440\u043e\u0432\u043d\u043e";
    }
    inline const char* finalWin() {
#ifdef _WIN32
        if (!console::is_real_console_stdout()) return "Total: you won ";
#endif
        return u8"\u0418\u0442\u043e\u0433: \u0432\u044b \u0432\u044b\u0438\u0433\u0440\u0430\u043b\u0438 ";
    }
    inline const char* finalLose() {
#ifdef _WIN32
        if (!console::is_real_console_stdout()) return "Total: you lost ";
#endif
        return u8"\u0418\u0442\u043e\u0433: \u0432\u044b \u043f\u0440\u043e\u0438\u0433\u0440\u0430\u043b\u0438 ";
    }
    inline const char* finalSame() {
#ifdef _WIN32
        if (!console::is_real_console_stdout()) return "Total: no changes. Balance: ";
#endif
        return u8"\u0418\u0442\u043e\u0433: \u0431\u0435\u0437 \u0438\u0437\u043c\u0435\u043d\u0435\u043d\u0438\u0439. \u0412\u0441\u0435\u0433\u043e: ";
    }
    inline const char* totalPrefix() {
#ifdef _WIN32
        if (!console::is_real_console_stdout()) return ". Balance: ";
#endif
        return u8". \u0412\u0441\u0435\u0433\u043e: ";
    }
} // namespace text

// -------------------- Adapter task --------------------
struct IFormattable {
    virtual ~IFormattable() = default;
    virtual std::string format() const = 0;
};

void prettyPrint(const IFormattable& object) {
    std::cout << object.format() << "\n";
}

// -------------------- Cards --------------------
enum class Suit { Spades, Hearts, Diamonds, Clubs };

static inline const char* suitSymbolUTF8(Suit s) {
    // Unicode: ♥ ♦ ♣ ♠
    switch (s) {
    case Suit::Hearts:   return u8"\u2665";
    case Suit::Diamonds: return u8"\u2666";
    case Suit::Clubs:    return u8"\u2663";
    case Suit::Spades:   return u8"\u2660";
    }
    return "?";
}

static inline const char* suitSymbolASCII(Suit s) {
    switch (s) {
    case Suit::Hearts:   return "H";
    case Suit::Diamonds: return "D";
    case Suit::Clubs:    return "C";
    case Suit::Spades:   return "S";
    }
    return "?";
}

static inline const char* suitSymbol(Suit s) {
#ifdef _WIN32
    if (!console::is_real_console_stdout()) return suitSymbolASCII(s);
#endif
    return suitSymbolUTF8(s);
}

struct Card {
    std::string rank;
    Suit suit{ Suit::Spades };

    Card() = default;
    Card(std::string r, Suit s) : rank(std::move(r)), suit(s) {}

    bool isAce() const { return rank == "A"; }

    int baseValue() const {
        if (rank == "A") return 11;
        if (rank == "K" || rank == "Q" || rank == "J") return 10;
        return std::stoi(rank);
    }

    friend std::ostream& operator<<(std::ostream& os, const Card& c) {
        os << c.rank << suitSymbol(c.suit);
        return os;
    }
};

// -------------------- Deck / Shoe / Hand (STL collections) --------------------
class Deck {
public:
    explicit Deck(int deckSize = 52) { reset(deckSize); }

    void reset(int deckSize) {
        if (deckSize != 52 && deckSize != 36) throw std::invalid_argument("Deck size must be 36 or 52");
        fullSize_ = deckSize;
        cards_.clear();

        static const std::array<Suit, 4> suits{ Suit::Spades, Suit::Hearts, Suit::Diamonds, Suit::Clubs };

        std::vector<std::string> ranks;
        if (deckSize == 52) ranks = { "2","3","4","5","6","7","8","9","10","J","Q","K","A" };
        else ranks = { "6","7","8","9","10","J","Q","K","A" };

        for (Suit s : suits) {
            for (const auto& r : ranks) cards_.emplace_back(r, s);
        }
        shuffle();
    }

    void shuffle() {
        static thread_local std::mt19937 rng{ std::random_device{}() };
        std::shuffle(cards_.begin(), cards_.end(), rng);
    }

    bool empty() const { return cards_.empty(); }
    size_t size() const { return cards_.size(); }
    int fullSize() const { return fullSize_; }

    Card draw() {
        if (cards_.empty()) throw std::runtime_error("Deck is empty");
        Card c = cards_.back();
        cards_.pop_back();
        return c;
    }

    std::vector<Card> topCards(size_t n) const {
        std::vector<Card> out;
        n = std::min(n, cards_.size());
        out.reserve(n);
        std::copy_n(cards_.rbegin(), static_cast<std::ptrdiff_t>(n), std::back_inserter(out));
        return out;
    }

private:
    int fullSize_{ 52 };
    std::deque<Card> cards_;
};

class Shoe {
public:
    Shoe(int deckCount, int deckSize) : rng_(std::random_device{}()) {
        if (deckCount <= 0) throw std::invalid_argument("deckCount must be > 0");
        decks_.reserve(static_cast<size_t>(deckCount));
        for (int i = 0; i < deckCount; ++i) decks_.emplace_back(deckSize);
    }

    Card draw() {
        std::vector<size_t> nonEmpty;
        nonEmpty.reserve(decks_.size());
        for (size_t i = 0; i < decks_.size(); ++i) {
            if (!decks_[i].empty()) nonEmpty.push_back(i);
        }

        if (nonEmpty.empty()) {
            int deckSize = decks_.empty() ? 52 : decks_[0].fullSize();
            *this = Shoe(static_cast<int>(decks_.size()), deckSize);
            for (size_t i = 0; i < decks_.size(); ++i) nonEmpty.push_back(i);
        }

        std::uniform_int_distribution<size_t> dist(0, nonEmpty.size() - 1);
        return decks_[nonEmpty[dist(rng_)]].draw();
    }

    std::string countsLine() const {
        std::ostringstream ss;
        ss << "Decks:";
        for (const auto& d : decks_) ss << " [" << d.size() << "]";
        return ss.str();
    }

    const Deck& deck(size_t i) const { return decks_.at(i); }

private:
    std::mt19937 rng_;
    std::vector<Deck> decks_;
};

class BustException : public std::runtime_error {
public:
    explicit BustException(int score) : std::runtime_error("Bust"), score_(score) {}
    int score() const { return score_; }
private:
    int score_{ 0 };
};

class Hand {
public:
    void clear() { cards_.clear(); }

    void addCard(const Card& c) {
        cards_.push_back(c);
        if (score() > 21) throw BustException(score());
    }

    size_t size() const { return cards_.size(); }
    const std::vector<Card>& cards() const { return cards_; }

    int score() const {
        const int total = std::accumulate(cards_.begin(), cards_.end(), 0,
            [](int acc, const Card& c) { return acc + c.baseValue(); });

        int aces = static_cast<int>(std::count_if(cards_.begin(), cards_.end(),
            [](const Card& c) { return c.isAce(); }));

        int t = total;
        while (t > 21 && aces > 0) {
            t -= 10;
            --aces;
        }
        return t;
    }

    bool isBlackjack21() const { return cards_.size() == 2 && score() == 21; }

    // Variant 5 special rule: "17 + 4"
    bool isSpecial17Plus4() const { return cards_.size() == 4 && score() == 17; }

    std::string toString(bool hideSecondCard = false) const {
        std::ostringstream ss;
        for (size_t i = 0; i < cards_.size(); ++i) {
            if (hideSecondCard && i == 1) ss << "??";
            else ss << cards_[i];
            if (i + 1 != cards_.size()) ss << " ";
        }
        return ss.str();
    }

    Card takeCard(size_t idx) {
        Card c = cards_.at(idx);
        cards_.erase(cards_.begin() + static_cast<std::ptrdiff_t>(idx));
        return c;
    }

private:
    std::vector<Card> cards_;
};

// -------------------- Participants --------------------
class Player {
public:
    explicit Player(long long money = 10000) : money_(money) {}

    long long money() const { return money_; }
    void addMoney(long long delta) { money_ += delta; }

    void resetHands() {
        hands_.assign(1, Hand{});
        busted_.assign(1, false);
    }

    std::vector<Hand>& hands() { return hands_; }
    const std::vector<Hand>& hands() const { return hands_; }

    bool isBusted(size_t i) const { return busted_.at(i); }
    void setBusted(size_t i, bool v) { busted_.at(i) = v; }

    bool canSplitFirstHand() const {
        if (hands_.size() != 1) return false;
        const auto& h = hands_[0];
        if (h.size() != 2) return false;
        return h.cards()[0].rank == h.cards()[1].rank;
    }

    void doSplit() {
        if (!canSplitFirstHand()) return;
        Hand second;
        Card c2 = hands_[0].takeCard(1);
        second.addCard(c2);
        hands_.push_back(second);
        busted_.push_back(false);
    }

private:
    long long money_{ 10000 };
    std::vector<Hand> hands_{ Hand{} };
    std::vector<bool> busted_{ false };
};

class Dealer {
public:
    void reset() { hand_.clear(); busted_ = false; }
    Hand& hand() { return hand_; }
    const Hand& hand() const { return hand_; }
    bool busted() const { return busted_; }
    void setBusted(bool v) { busted_ = v; }
private:
    Hand hand_;
    bool busted_{ false };
};

// -------------------- GOF Adapter --------------------
class DeckObjectAdapter : public IFormattable {
public:
    explicit DeckObjectAdapter(const Deck& deck) : deck_(deck) {}
    std::string format() const override {
        std::ostringstream ss;
        ss << "[Deck (object-adapter)] remaining=" << deck_.size() << ", top:";
        for (const auto& c : deck_.topCards(5)) ss << " " << c;
        return ss.str();
    }
private:
    const Deck& deck_;
};

class DeckClassAdapter : public Deck, public IFormattable {
public:
    explicit DeckClassAdapter(int deckSize = 52) : Deck(deckSize) {}
    std::string format() const override {
        std::ostringstream ss;
        ss << "[Deck (class-adapter)] remaining=" << size() << ", top:";
        for (const auto& c : topCards(5)) ss << " " << c;
        return ss.str();
    }
};

// -------------------- Game --------------------
class Game {
public:
    Game() : shoe_(4, 52), player_(10000) {}

    void run() {
        cout << text::header();
        cout << text::adapterDemo();
        DeckObjectAdapter oa(shoe_.deck(0));
        prettyPrint(oa);
        DeckClassAdapter ca(52);
        prettyPrint(ca);
        cout << "\n";

        while (player_.money() > 0) {
            const long long bet = askBet();
            playRound(bet);

            cout << text::playAgain();
            int again = 0;
            cin >> again;
            if (!cin || again == 0) break;
        }

        cout << text::gameOver() << player_.money() << "\n";
    }

private:
    long long askBet() {
        while (true) {
            cout << text::betAsk();
            long long bet = 0;
            cin >> bet;

            if (!cin) {
                cin.clear();
                cin.ignore(std::numeric_limits<std::streamsize>::max(), '\n');
                continue;
            }
            if (bet <= 0) continue;
            if (bet > player_.money()) {
                cout << text::notEnoughMoney() << player_.money() << "\n";
                continue;
            }
            return bet;
        }
    }

    void initialDealBasic() {
        dealer_.reset();
        player_.resetHands();

        dealer_.hand().addCard(shoe_.draw());
        dealer_.hand().addCard(shoe_.draw());

        player_.hands()[0].addCard(shoe_.draw());
        player_.hands()[0].addCard(shoe_.draw());
    }

    void showTableState(bool revealDealer = false) const {
        cout << shoe_.countsLine() << "\n";
        cout << text::dealerLabel() << dealer_.hand().toString(!revealDealer) << "\n";

        if (player_.hands().size() == 1) {
            cout << text::youLabel() << player_.hands()[0].toString(false) << "\n";
        }
        else {
            for (size_t i = 0; i < player_.hands().size(); ++i) {
                cout << text::handLabel() << (i + 1) << "): " << player_.hands()[i].toString(false) << "\n";
            }
        }
    }

    void playPlayerHands() {
        for (size_t handIdx = 0; handIdx < player_.hands().size(); ++handIdx) {
            while (true) {
                auto& hand = player_.hands()[handIdx];

                if (hand.isBlackjack21()) { cout << text::blackjackMsg(); break; }
                if (hand.isSpecial17Plus4()) { cout << text::specialMsg(); break; }

                showTableState(false);

                cout << text::stopMenu();
                cout << text::hitMenu();
                if (handIdx == 0 && player_.canSplitFirstHand()) cout << text::splitMenu();

                int choice = 0;
                cin >> choice;
                if (!cin) {
                    cin.clear();
                    cin.ignore(std::numeric_limits<std::streamsize>::max(), '\n');
                    continue;
                }

                if (choice == 1) {
                    break;
                }
                else if (choice == 2) {
                    try {
                        hand.addCard(shoe_.draw());
                    }
                    catch (const BustException& ex) {
                        player_.setBusted(handIdx, true);
                        showTableState(false);
                        cout << text::bustMsg() << ex.score() << ")\n";
                        break;
                    }
                }
                else if (choice == 3 && handIdx == 0 && player_.canSplitFirstHand()) {
                    player_.doSplit();
                    for (size_t i = 0; i < player_.hands().size(); ++i) {
                        try {
                            player_.hands()[i].addCard(shoe_.draw());
                        }
                        catch (const BustException& ex) {
                            player_.setBusted(i, true);
                            cout << text::bustMsg() << ex.score() << ")\n";
                        }
                    }
                }
            }
        }
    }

    void playDealerBasic() {
        showTableState(true);

        bool anyAlive = false;
        for (size_t i = 0; i < player_.hands().size(); ++i) {
            if (!player_.isBusted(i)) { anyAlive = true; break; }
        }
        if (!anyAlive) return;

        while (dealer_.hand().score() < 17) {
            cout << text::dealerTakes();
            try {
                dealer_.hand().addCard(shoe_.draw());
            }
            catch (const BustException& ex) {
                dealer_.setBusted(true);
                showTableState(true);
                cout << text::dealerBust() << ex.score() << ")\n";
                return;
            }
            showTableState(true);
        }
    }

    enum class Outcome { Win, Lose, Push };

    Outcome compareHandToDealer(const Hand& ph) const {
        if (dealer_.busted()) return Outcome::Win;

        const int p = ph.score();
        const int d = dealer_.hand().score();

        const bool pBJ = ph.isBlackjack21() || ph.isSpecial17Plus4();
        const bool dBJ = dealer_.hand().isBlackjack21() || dealer_.hand().isSpecial17Plus4();

        if (pBJ && dBJ) return Outcome::Push;
        if (pBJ && !dBJ) return Outcome::Win;
        if (!pBJ && dBJ) return Outcome::Lose;

        if (p > d) return Outcome::Win;
        if (p < d) return Outcome::Lose;
        return Outcome::Push;
    }

    void settleBet(long long bet) {
        const bool split = (player_.hands().size() == 2);

        std::vector<Outcome> outcomes;
        outcomes.reserve(player_.hands().size());

        for (size_t i = 0; i < player_.hands().size(); ++i) {
            if (player_.isBusted(i)) outcomes.push_back(Outcome::Lose);
            else outcomes.push_back(compareHandToDealer(player_.hands()[i]));
        }

        const int wins = static_cast<int>(std::count(outcomes.begin(), outcomes.end(), Outcome::Win));
        const int losses = static_cast<int>(std::count(outcomes.begin(), outcomes.end(), Outcome::Lose));
        const int pushes = static_cast<int>(std::count(outcomes.begin(), outcomes.end(), Outcome::Push));

        long long delta = 0;
        if (!split) {
            if (outcomes[0] == Outcome::Win) delta = +bet;
            else if (outcomes[0] == Outcome::Lose) delta = -bet;
            else delta = 0;
        }
        else {
            // Split rule from lab: both lose -> -bet (NOT -2*bet)
            if (wins == 2) delta = +2 * bet;
            else if (losses == 2 && wins == 0 && pushes == 0) delta = -bet;
            else delta = static_cast<long long>(wins - losses) * bet;
        }

        player_.addMoney(delta);

        for (size_t i = 0; i < outcomes.size(); ++i) {
            cout << text::handWord() << (i + 1) << ": ";
            if (player_.isBusted(i)) {
                cout << text::bustWord() << " -> " << text::loseWord() << "\n";
                continue;
            }
            if (outcomes[i] == Outcome::Win) cout << text::winWord() << "\n";
            else if (outcomes[i] == Outcome::Lose) cout << text::loseWord() << "\n";
            else cout << text::pushWord() << "\n";
        }

        if (delta > 0) cout << text::finalWin() << delta << text::totalPrefix() << player_.money() << ".\n";
        else if (delta < 0) cout << text::finalLose() << (-delta) << text::totalPrefix() << player_.money() << ".\n";
        else cout << text::finalSame() << player_.money() << ".\n";
    }

    void playRound(long long bet) {
        initialDealBasic();
        playPlayerHands();
        playDealerBasic();

        cout << text::roundResult();
        showTableState(true);

        cout << text::dealerScore() << dealer_.hand().score() << "\n";
        for (size_t i = 0; i < player_.hands().size(); ++i) {
            cout << text::playerScorePrefix() << (i + 1) << "): ";
            if (player_.isBusted(i)) cout << text::bustWord() << "\n";
            else cout << player_.hands()[i].score() << "\n";
        }

        settleBet(bet);
    }

private:
    Shoe shoe_;
    Player player_;
    Dealer dealer_;
};

// -------------------- main --------------------
int main() {
    console::enable_utf8_if_possible();

    try {
        Game g;
        g.run();
    }
    catch (const std::exception& ex) {
        std::cerr << "Fatal error: " << ex.what() << "\n";
        return 1;
    }
    return 0;
}
