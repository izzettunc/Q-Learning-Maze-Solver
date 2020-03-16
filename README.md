<br />
<p align="center">

  <h3 align="center">Q Learning Maze Solver</h3>

  <p align="center">
    A maze solver simulation using Q Learning algorithm.
    <br />
    <br />
    <a href="https://github.com/izzettunc/Q-Learning-Maze-Solver/issues">Report Bug</a>
    ·
    <a href="https://github.com/izzettunc/Q-Learning-Maze-Solver/issues">Request Feature</a>
  </p>
</p>



<!-- TABLE OF CONTENTS -->
## Table of Contents

* [About the Project](#about-the-project)
* [Getting Started](#getting-started)
  * [Installation](#installation)
* [Usage](#usage)
* [Roadmap](#roadmap)
* [License](#license)
* [Contact](#contact)



<!-- ABOUT THE PROJECT -->
## About The Project

![Product Name Screen Shot][product-screenshot]

This project made as a class assignment.It's purpose basically make the computer learn how to solve the maze with Q-Learning algorithm.

**Features:**

* You can run the simulation with and see what happens
* You can generate random mazes with prim algorithm for more info -> https://en.wikipedia.org/wiki/Maze_generation_algorithm#Randomized_Prim%27s_algorithm
* You can slow it down, make the maze bigger , play with some metrics
* You can print the Q Table when the desired epoch reached

### What is Q-Learning algorithm ?

Q-learning is a model-free reinforcement learning algorithm. The goal of Q-learning is to learn a policy, which tells an agent what action to take under what circumstances. It does not require a model (hence the connotation "model-free") of the environment, and it can handle problems with stochastic transitions and rewards, without requiring adaptations.

For any finite Markov decision process (FMDP), Q-learning finds a policy that is optimal in the sense that it maximizes the expected value of the total reward over any and all successive steps, starting from the current state. Q-learning can identify an optimal action-selection policy for any given FMDP, given infinite exploration time and a partly-random policy. "Q" names the function that returns the reward used to provide the reinforcement and can be said to stand for the "quality" of an action taken in a given state.

<!-- GETTING STARTED -->
## Getting Started

To get a local copy up and running follow these simple steps.

### Installation

1.  Clone the repo
```sh
git clone https://github.com/izzettunc/Q-Learning-Maze-Solver.git
```
2. Open the project with Unity

3. Make changes, run it, use it whatever you like :smile:

<!-- USAGE EXAMPLES -->
## Usage

* Select your settings and play start.
![Application Screen Shot][settings-screenshot]
* If you bored or want to change the settings then press back button on bottom left.
![Application Screen Shot][simulation-screenshot]

Unnecessary information for settings

* Maze Length : Changes size of the maze its range changes between 3 to 25 for best view experience.
* Select Random Start And End Point : Randomize start and end point.
* Step Per Second : Changes how much step will be processed for second you can speed it up by increasing or slow it down by keeping it in 0 - 1 range.
* Waiting Time At The End : Changes how much time the simulation is going to wait when character reaches the end.
* Reward : Reward value in the reward table for Q Learning algorithm.
* Learning Rate : https://en.wikipedia.org/wiki/Learning_rate ( Changes between 0 and 1 )
* Discount Rate : https://en.wikipedia.org/wiki/Q-learning#Discount_factor ( Changes between 0 and 1 )
* Epoch : Determines how many times character have to reach end. ( -1 for limitless epoch )
* Stop Learning At: Determines when the character stops learning. This setting has no relevance with the algorithm. I added it just for fun. (-1 for limitless)
* Activate Smart Choosing : Activates smart choosing. This setting has no relevance with the algorithm. I misunderstood the algorithm and look for one step further while selecting the best Q value and in the end I couldn't delete it and add it as a feature.

<!-- ROADMAP -->
## Roadmap

See the [open issues](https://github.com/izzettunc/Q-Learning-Maze-Solver/issues) for a list of proposed features (and known issues).

<!-- LICENSE -->
## License

Distributed under the MIT License. See `LICENSE` for more information.

<!-- CONTACT -->
## Contact

İzzet Tunç - izzet.tunc1997@gmail.com
<br>
[![LinkedIn][linkedin-shield]][linkedin-url]

[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=flat-square&logo=linkedin&colorB=555
[linkedin-url]: https://www.linkedin.com/in/izzettunc
[product-screenshot]: data/screenshots/header.png
[settings-screenshot]: data/screenshots/settings.png
[simulation-screenshot]: data/screenshots/simulation.png
