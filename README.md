# XPathRunner and XPathRunnerUI

Welcome to XPathRunner and XPathRunnerUI repository! 

## Overview

XPathRunner and XPathRunnerUI are two powerful tools for working with XPath expressions on HTML or XML files. 

- **XPathRunner**: A command-line application that allows you to evaluate XPath expressions directly on HTML or XML files. It's perfect for quick and efficient XPath evaluation in your scripts or terminal.

- **XPathRunnerUI**: A graphical user interface built with Avalonia framework, providing a user-friendly way to interactively evaluate XPath expressions. With XPathRunnerUI, you can easily load files, input XPath expressions, and see the results in a visually appealing interface.

## Features

### XPathRunner
- Evaluate XPath expressions on HTML or XML files from the command line.
- Supports both local and remote file paths.
- Provides clear and concise output of XPath evaluation results.

### XPathRunnerUI
- Intuitive graphical user interface for XPath expression evaluation.
- Load HTML or XML files with ease.
- Input XPath expressions and see the results instantly.
- Visual presentation of XPath evaluation results for better understanding.

## How to Use

### Clone the Repository
Clone the repository to your local machine:
```bash
git clone https://github.com/agailloty/xpathrunner.git
```
### Build XPathRunner
1. Navigate to the xpathrunner directory:
```bash
cd xpathrunner
```
2. Build the project:
```bash
dotnet build -c Release
```

## XPathRunner
Download the latest release binary for your platform from the Releases page.
Open your terminal or command prompt.
Navigate to the directory containing the downloaded binary.
Run xpathrunner file --filepath some/path/file.html --xpath "//span[starts-with(@id, 'Hello')]" to evaluate an XPath expression on a file.
## XPathRunnerUI
Download the latest release binary for your platform from the Releases page.
Double-click the executable file to launch the application.
Use the intuitive interface to load files and input XPath expressions.
View the results directly within the application window.

# Contributions
We welcome contributions from the community to improve XPathRunner and XPathRunnerUI. Whether it's fixing bugs, adding new features, or improving documentation, your contributions are highly appreciated. Please refer to our contribution guidelines for more information on how to get started.

# License
This project is licensed under the MIT License, which means you are free to use, modify, and distribute the software as long as you include the original copyright and license notice.

# Feedback and Support
If you have any feedback, suggestions, or encounter any issues while using XPathRunner or XPathRunnerUI, please feel free to open an issue on GitHub. We are committed to providing the best possible experience for our users and appreciate any feedback that helps us achieve that goal.

Happy XPathing! 🚀

