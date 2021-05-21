n = prompt_i("n: ");

for i = 0 to 9
    print(fibonacci(i));

int fibonacci(int i) {
    if (i == 0 || i == 1)
        return i;
    return fibonacci(i - 1) + fibonacci(i - 2);
}