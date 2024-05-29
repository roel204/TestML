# Unity ML Agents Fish Project

This repository contains a Unity project for training a fish agent using ML-Agents.

## Setup Instructions

Follow these steps to set up the project on your local machine.

### Prerequisites

- [Unity](https://unity.com/) Use version: 2023.1.20f1
- [Python](https://www.python.org/downloads/release/python-379/) Use version: 3.7.9 | Make sure you get X86-64 for windows
- [Git](https://git-scm.com/)

### Installation

1. **Clone the Repository and open the folder in CMD**
   
   `cd C:...\TestML`
   
   (Or type "cmd" in Windows file explorer while you have the folder open.)

2. **Create a Virtual Environment**
   
   `py -m venv venv`

3. **Activate the Virtual Environment**

   `venv\Scripts\activate`

4. **Update pip**
   
   `py -m pip install --upgrade pip`

5. **Install Dependencies**
   
   ```
   pip3 install torch==1.7.0 -f https://download.pytorch.org/whl/torch_stable.html
   pip3 install mlagents
   pip install six
   pip install importlib-metadata==4.8
   ```

6. **Open the Project in Unity**


## Usage

Make sure you're inside the venv!

To train the model use: `mlagents-learn --run-id=YourID`

Add `--force` to override previous training.

Add `--resume` to continue from previous training

Use `mlagents-learn --help` to see all other commands
