clear all; close all; clc;

% Import image and display
I = imread('..\images\Monkey.png');

test = Snake();

test.setRawImage(I);

% Add some snake points
test.addSnakePoint([124;106]);
test.addSnakePoint([124;163]);
test.addSnakePoint([196;163]);
test.addSnakePoint([196;106]);

test.doubleSnakePoints();
test.doubleSnakePoints();

for i = 1:100;
    test.applyForces();
    test.showImage();    
end

test.showImage();



test


%% test

