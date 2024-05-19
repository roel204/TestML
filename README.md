# Unity ML Agents Fish Project

This repository contains a Unity project for training a fish agent using ML-Agents.

## Setup Instructions

Follow these steps to set up the project on your local machine.

### Prerequisites

- [Unity](https://unity.com/) (Ensure you have the version that matches the project)
- [Python](https://www.python.org/downloads/) (Ensure you have Python 3.6 or newer)
- [Git](https://git-scm.com/)

### Installation

1. **Clone the Repository and open the folder in CMD**
   
   `cd C:...\TestML`
   
   (Or type "cmd" in Windows file explorer while you have the folder open.)

2. **Create a Virtual Environment**
   
   `py -m venv venv`

3. **Activate the Virtual Environment**

   `venv\Scripts\activate`

4. **Install Dependencies**

    `pip install -r requirements.txt`

5. **Open the Project in Unity**


## Usage

To train the model run:

`mlagents-learn --run-id=1`

(Make sure you choose an ID.)

Use `--force` to override previous training.
