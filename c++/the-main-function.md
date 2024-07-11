# The `main` function

When we write program using C, C++, Go, Java and many others, we mostly will interact with this function. Some libraries or frameworks minimize the interaction though, to let programmers to focus to the business layer. However, they do not replace the `main` function. In this article I'm trying to document my learning process about the `main` function, which I used to just use it without any deeper concern.

## What is it?
The `main` function is the entry-point of a program, where the instructions to be executed. However this is not the case with libraries, they don't have one. What if a program doesn't have one? It simply won't compile.

## How is it declared?
Generally the `main` function is declared like the following:
```c++
int main() { ... }

// or

int main(int argc, char* argv[]) { ... }

// or, with common POSIX extension

int main(int argc, char* argv[], char* envp[]) { ... }
```

### The name `main`
The name `main` was inherited from C, and it escapes from all the revision and changes between standards, except got a little bit annoyance in MSVC which they introduce `wmain` for arguments that supports wide chars.

### The signature
The signature was also inherited, this time from the old days of *C++98*, and the third version also accomodating POSIX extension. The following would be the explanation of the signature elements.

#### `int` return value
This is the return value that C++98 and above expects. However in the early days, we've seen something like `void main(...)` things. The `int` return type, as I've read somewhere, while it does not direct mapping with the OS process exit statuses, it's for easy conversion and more intuitively interpretable. But I think it's more of a guess of mine, if you know the reason behind it, please let me know.

#### `int argc` argument
This is the count of entire argument including the executable name itself. Thus,
```bash
./cut -d' ' -f2,3,4 data.csv 
```
will have `argc` equals to 4.

#### `char* argv[]` argument
This is the argument values, packed in array. The first element of the array will always be the program name. Hence, from the previous example it will be equal to `./cut`, and the next in sequence will be `-d' '`, `-f2,3,4` and the last one will be `data.csv`. What about the next element, `argv[argc]`? It is guaranteed to be `null`.

#### `char* envp[]` argument
This argument does not come from the C++ standard, but commonly used and as part of the POSIX extension. In UNIX like OS it's not uncommon to provide on-demand environment variables when executing a program like this:
```bash
EDITOR=/bin/vim visudo
```
This is where the `envp` parameter came to play. Since environment variable can be passed or not by user, this array has the possibility to be empty. Also, because of the absence of the count parameter (like `argc` to `argv`), programmer shall loop through the array until finds `null` which is the indicator that there's no more value to collect. 

## How does it behave?
## What can we do with it?
## What can NOT we do with it?
## Summary
## Reference
- [Daily bit(e) of C++ | `main`](https://simontoth.substack.com/p/daily-bite-of-c-main?utm_source=post-email-title&publication_id=1177271&post_id=143546210&utm_campaign=email-post-title&isFreemail=true&r=2a3bae&triedRedirect=true)
- [`main` function and command-line arguments](https://learn.microsoft.com/en-us/cpp/cpp/main-function-command-line-args?view=msvc-170)
- [Main function](https://en.cppreference.com/w/c/language/main_function)
