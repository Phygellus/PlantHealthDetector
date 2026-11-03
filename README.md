# **PLANT HEALTH DETECTOR**
### **PROJECT OVERVIEW:**
Description: A small and simple neural network program where the programs is trained to detect whether a plant is healthy or unhealthy (possibly diseased).

Input Layer: **3072 inputs** (uses 32x32 RGB images)  
Hidden Layer: **16 Neurons** (optimal for tuning)   
Output Layer: **1 output** (healthy | unhealthy)   
Learning Rate: **0.15** (optimal)   
Epochs: **100 epochs** (optimal)

[Training Log](https://cebuinstituteoftechnology-my.sharepoint.com/:u:/g/personal/larainekeisha_capacio_cit_edu/IQBiH1_6Av-nTICcJqvHOl0sAWLBBKM4AAn3cfvhR150FOM?e=lvdTlT)

### **DATA DESCRIPTION:**
The dataset used to train the neural network is a folder of 1000 images that was randomly sampled from a 175k dataset in Kaggle / New Plant Diseases Dataset containing the difference between a healthy and unhealthy leaf of a plant. The dataset was split evenly into 500 images of healthy leaves and 500 images of unhealthy leaves randomly chosen from the folder.

[[dataset.zip](https://cebuinstituteoftechnology-my.sharepoint.com/:u:/g/personal/larainekeisha_capacio_cit_edu/IQDGjMf-h7kbT5IR1p2x96oHAaO4VSASKSaY_1j9jYSEJHQ?e=by2Eeo)]
___

### **HOW TO RUN:**
1.	Clone/Fork the PlantHealthDetector repository.
2.	Open up Visual Studio, click new repository and click PlantHealthDetector.
3.	Downoad the dataset from above paste it in the root directory.
4.  (Recommended) Change from debug to release mode for a faster performance.
5.  Run the program.
6.  Click the buttons under NeuralNetwork in the Menu in order:
    - Preparation
    - Train Model (may be more than once)
    - Test Run
7. (Recommended) Use [Training.zip](https://cebuinstituteoftechnology-my.sharepoint.com/:u:/g/personal/larainekeisha_capacio_cit_edu/IQBiH1_6Av-nTICcJqvHOl0sAWLBBKM4AAn3cfvhR150FOM?e=lvdTlT) to test how well the program does with images not in the dataset.
8. Open an image from the Training folder.
9. Click "Manual Training" under NeuralNetwork to run the training.
10. Be sure to confirm whether or not the program correctly detected its health.
11. (Recommended) Export the training log and weights for future studies and uses.
___
## **Documentation**
### Data Processing
After getting the 1000 images, these images were resized into 32x32 and flattened into a 1D arrray for 3072 pixels (32 \* 32 \* 3 = 3072). The program then reads and identifies which are healthy and which are unhealthy (under the folder names). Here, it sorts them between a healthy list and unhealthy list with labels (0 being healthy and 1 being unhealthy). The data is then shuffled to prevent a possible data bias when training and testing. The program then splits the images into 80/20, with 80% being used for training while the other 20% is used for testing. Pixel values were also normalized from 0-255 values to either 0 or 1 to prevent Sigmoid Saturation.
### Training
The program used binary classification during training to classify whether a plant is healthy or not, using the labels (0.0 and 1.0) as the desired output for each image.

The program also uses shuffling to make the learning much easier, and used a method called early stopping to make sure it stops the epoch loop whenever the accuracy stays the same for 15 or more epochs.
### Hyperparameter Tuning
As seen in the Training Log, I tested and tuned out the parameters to find the most optimal way to run the program. The hidden neurons I tested out ranged from 4, 16, 32, and 64 while I used 0.1 and 0.15 for learning rate. I came into the conclusion that 16 hidden neurons and a 0.15 learning rate would be most optimal parameters for this program as it was the one that reached a peak height of 76% accuracy. I also changed the epochs from 1000 to 100 because the average epochs that were used before the early stopping gets triggered is less than a hundred but more than 50 epochs.
