a = prompt_i("a: ");
b = prompt_i("b: ");
print(gcd(a, b));

int gcd(int a, int b) {
    min = a < b ? a : b;
    for i = min downto 1 {
        // print(i + " " + a % i + " " + b % i + " " + (a % i == 0 && b % i == 0));
        if (a % i == 0 && b % i == 0)
            return i;
    }
    return 1;
}