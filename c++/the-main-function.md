# The C++ `main` function

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
The name `main` was inherited from C, and it escapes from all the revision and changes between standards, except got a little bit annoyance in MSVC++ which they introduce `wmain` for arguments that supports wide chars.

### The signature
The signature was also inherited, this time from the old days of *C++98*, and the third version also accomodating POSIX extension. The following would be the explanation of the signature elements.

#### `int` return value
> In the early days, we've seen something like `void main(...)`. However, using it in modern C++ will raise an error.

This is the return value that C++98 and above expects. The `int` return type, as I've read somewhere, while it does not direct mapping with the OS process exit statuses, it's for easy conversion and more intuitively interpretable. But I think it's more of a guess of mine, if you know the reason behind it, please let me know.

#### `int argc` argument
This is the count of entire argument including the executable name itself. Thus,
```bash
./cut -d' ' -f2,3,4 data.csv 
```
will have `argc` equals to 4.

#### `char* argv[]` argument
This is the argument values, packed in array. The first element of the array will always be the program name. Hence, from the previous example it will be equal to `./cut`, and the next in sequence will be `-d' '`, `-f2,3,4` and the last one will be `data.csv`. What about the next element, `argv[argc]`? It is guaranteed to be `null`.

#### `char* envp[]` argument
This argument does not come from the C++ standard, but commonly used and as part of the POSIX extension. This parameter will be the storage of the frozen version on user's environment variables, including those provided when executing a program like this:
```bash
EDITOR=/bin/vim visudo
```
The `envp` array has the possibility to be empty. Also, because of the absence of the count parameter (like `argc` to `argv`), programmer shall loop through the array until finds `null` which is the indicator that there's no more value to collect. 

However based on many discussion on the internet, many of them discourage to use this as it's not portable between systems. Some also pointed out about overflowing possibilities. I'd like to hear from you too if you have something in mind about this.

## How does it flow?
### What happened before `main` begins?
There are some activities happens when we executed a program. The following are the activities that is performed before it entering the `main` function.

- Linker takes the object files and libraries the program linked against, and combined them into an executable program. After that, loader will load the program into the memory.
- Memory is allocated both for the code, data and stack.
- Global variables are initialized with their corresponding initializers, and for the uninitialized, they will be set to zeros. On MSVC++, all static members of classes will also be initialized here similar to global variables. However, other compilers decide to perform this at compile time.
- Function pointers are set up, and virtual tables are constructed for *dynamic dispatch*. 
- Entering the executable entry point as OS expects it, that is the `_start` section in the assembly code version of our program. In this section, it will initialize C++ runtime library required for running the program, and also prepare the arguments for our actual `main` function and finally invoke it.

### What happened after `main` ends?
- The destructor of all globals and static objects will be called in reversed order as they were created to clean up the resources they may allocated.
- After that the C++ runtime library will make system call to OS to indicate that the program is terminating, typically will take the `main` return value as its parameter.
- OS will then update the process status to terminated and marks it with the `main` function return value.

## What can we do with it?
- Declare the function body as `try/catch` block:
```c++
// this example is borrowed from reference 1
#include <stdexcept>
#include <iostream>

struct Q {
    ~Q() noexcept(false) {
        throw std::runtime_error("will escape");
    }
} q;

int main() try {
    throw std::runtime_error("will be caught");
} catch (const std::exception& e) {
    // will caught the runtime error from main, but not from ~Q
    std::cerr << "caught: " << e.what() << std::endl;
}
```
here's the output of the above program:
```
caught: will be caught
terminate called after throwing an instance of 'std::runtime_error'
  what():  will escape
make: *** [Makefile:14: try-main.x] Aborted (core dumped)
```
A little explanation of the above code:
- entering `main`
- throw exception, hopefully be caught
- catching exception: yes, it is caught
- is there user code remaing to execute? no
- `main` exiting...
- program do some house keeping, found `q` of type `Q`, which throws exception in its d'tor
- destroy `q`, hence, throws exception `"will escape"`
- is there anymore to clean up? no
- call terminate

## What can NOT we do with it?
### Naming
- Cannot be named, thus can't take its address, can't also be called recursively, or get the type of it.
- Cannot use the name main for other function in global namespace or anything with C-linkage

### Construction
`main` can't be declared as `inline`, `static`, `constexpr`, `consteval` or with `auto` or as a coroutine. 

## Reference
1. [Daily bit(e) of C++ | `main`](https://simontoth.substack.com/p/daily-bite-of-c-main?utm_source=post-email-title&publication_id=1177271&post_id=143546210&utm_campaign=email-post-title&isFreemail=true&r=2a3bae&triedRedirect=true)
2. [`main` function and command-line arguments](https://learn.microsoft.com/en-us/cpp/cpp/main-function-command-line-args?view=msvc-170)
3. [Main function](https://en.cppreference.com/w/c/language/main_function)
