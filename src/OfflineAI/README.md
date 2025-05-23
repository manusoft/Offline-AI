# OFFILINE AI PROJECT
Using Llama.cpp to run TinyLlama locally on Windows and Ubuntu.

## SETUP
### SYSTEM REQUIREMENTS
- Windows 10 or later (64-bit)
- Ubuntu 20.04 or later (64-bit)
- 8GB RAM or more
- 4GB VRAM or more (for GPU acceleration)
- Python 3.8 or later (for Python scripts)
- CMake 3.10 or later (for building from source)
- Visual Studio 2019 or later (for Windows)
- GCC 7.5 or later (for Ubuntu)
- CUDA 10.2 or later (for GPU acceleration)
- cuDNN 7.6 or later (for GPU acceleration)
- NVIDIA driver 450.80.02 or later (for GPU acceleration)
- OpenCL 1.2 or later (for GPU acceleration)
- OpenMP 4.5 or later (for parallel processing)

### DOWNLOAD MODEL (TinyLlama)
#### Link
- https://huggingface.co/TheBloke/TinyLlama-1.1B-Chat-v1.0-GGUF/tree/main

#### Files
- tinyllama-1.1b-chat-v1.0.Q8_0.gguf (latest version)

## DOWNLOAD CLI (llama.cpp)
### Links
- https://github.com/ggml-org/llama.cpp
- https://github.com/ggml-org/llama.cpp/releases
### Files
- llama-b5468-bin-win-cpu-x64.zip - for windows
- llama-b5468-bin-ubuntu-x64.zip	- for ubuntu

## CREATE FOLDER (OfflineAI)
### EXTRACT llama-b5468-bin-win-cpu-x64.zip (windows)
- Copy all DLL files to the folder OfflineAI
- Copy llama-cli.exe to the folder OfflineAI
- Copy tinyllama-1.1b-chat-v1.0.Q8_0.gguf to the folder OfflineAI

## OPEN TERMINAL
### Run 
- .\llama-cli.exe --model .\tinyllama-1.1b-chat-v1.0.Q8_0.gguf --prompt "Hello, how are you doing today?" --n-predict 128 

## LIST OF COMMANDS
### --model
- Path to the model file (e.g., tinyllama-1.1b-chat-v1.0.Q8_0.gguf)
- Example: --model .\tinyllama-1.1b-chat-v1.0.Q8_0.gguf
- Default: None
- Required: Yes
- Type: String
- Description: The path to the model file to be used for inference. This is a required argument and must be provided in order to run the program.
- Note: The model file should be in the same directory as the llama-cli.exe file or provide the full path to the model file. The model file should be in GGUF format.

### --prompt
- The prompt to be used for inference (e.g., "Hello, how are you doing today?")
- Example: --prompt "Hello, how are you doing today?"
- Default: None
- Required: Yes
- Type: String
- Description: The prompt to be used for inference. This is a required argument and must be provided in order to run the program.
- Note: The prompt should be a string enclosed in double quotes. If the prompt contains double quotes, they should be escaped with a backslash (e.g., "Hello, how are you doing today?").

### --n-predict
- The number of tokens to predict (e.g., 128)
- Example: --n-predict 128
- Default: 128
- Required: No
- Type: Integer
- Description: The number of tokens to predict. This is an optional argument and defaults to 128 if not provided.
- Note: The number of tokens to predict is the maximum number of tokens that the model will generate in response to the prompt. The model may generate fewer tokens than specified, depending on the prompt and the model's internal logic.

### --top-k
- The number of top-k tokens to sample from (e.g., 40)
- Example: --top-k 40
- Default: 40
- Required: No
- Type: Integer
- Description: The number of top-k tokens to sample from. This is an optional argument and defaults to 40 if not provided.
- Note: The top-k sampling method is a technique used in natural language processing to select the most likely tokens from a set of candidates. The model will sample from the top-k tokens based on their probabilities. A higher value for top-k will result in more diverse outputs, while a lower value will result in more focused outputs.

### --top-p
- The cumulative probability threshold for nucleus sampling (e.g., 0.95)
- Example: --top-p 0.95
- Default: 0.95
- Required: No
- Type: Float
- Description: The cumulative probability threshold for nucleus sampling. This is an optional argument and defaults to 0.95 if not provided.
- Note: The nucleus sampling method is a technique used in natural language processing to select the most likely tokens from a set of candidates based on their cumulative probabilities. The model will sample from the tokens that have a cumulative probability greater than or equal to the specified threshold. A higher value for top-p will result in more diverse outputs, while a lower value will result in more focused outputs.

### --temperature
- The temperature for sampling (e.g., 0.7)
- Example: --temperature 0.7
- Default: 0.7
- Required: No
- Type: Float
- Description: The temperature for sampling. This is an optional argument and defaults to 0.7 if not provided.
- Note: The temperature parameter is a hyperparameter used in natural language processing to control the randomness of the model's output. A higher temperature will result in more random outputs, while a lower temperature will result in more focused outputs. The temperature value should be between 0 and 1, with 0 being the most focused and 1 being the most random.

### --seed
- The random seed for sampling (e.g., 42)
- Example: --
- seed 42
- Default: 42
- Required: No
- Type: Integer
- Description: The random seed for sampling. This is an optional argument and defaults to 42 if not provided.
- Note: The random seed parameter is used to initialize the random number generator used in the sampling process. This allows for reproducibility of the results. If the same seed is used, the same output will be generated for the same prompt and model. A different seed will result in a different output for the same prompt and model.

### EXAMPLES
- Example 1: .\llama-cli.exe --model .\tinyllama-1.1b-chat-v1.0.Q8_0.gguf --prompt "Hello, how are you doing today?" --n-predict 128 --top-k 40 --top-p 0.95 --temperature 0.7 --seed 42
- Example 2: .\llama-cli.exe --model .\tinyllama-1.1b-chat-v1.0.Q8_0.gguf --prompt "What is the capital of France?" --n-predict 64 --top-k 20 --top-p 0.9 --temperature 0.5 --seed 1234
- Example 3: .\llama-cli.exe --model .\tinyllama-1.1b-chat-v1.0.Q8_0.gguf --prompt "Tell me a joke." --n-predict 256 --top-k 50 --top-p 0.8 --temperature 0.9 --seed 5678
- Example 4: .\llama-cli.exe --model .\tinyllama-1.1b-chat-v1.0.Q8_0.gguf --prompt "What is the meaning of life?" --n-predict 128 --top-k 30 --top-p 0.95 --temperature 0.6 --seed 9999
- Example 5: .\llama-cli.exe --model .\tinyllama-1.1b-chat-v1.0.Q8_0.gguf --prompt "What is the weather like today?" --n-predict 64 --top-k 10 --top-p 0.85 --temperature 0.4 --seed 1111