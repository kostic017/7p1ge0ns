int gcd(int a, int b) {
    int min = a < b ? a : b
    for i = min downto 1 {
        if (a % i == 0 && b % i == 0)
            return i
    return 1
}

void main() {
    a = 0
    b = 0

    prompt("a: ", a)
    prompt("b: ", b)

    print(gcd(a, b))
}