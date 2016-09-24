classdef Snake < handle
    %SNAKE provides example functions for the Snake Algorighms.
    %   Detailed explanation goes here
    
    properties
        snakePoints = []
        rawImage
        displayImage
        grayImage
        smoothGrayImage
        smoothGrayEdgeImage
        forcesImage
        gaussSigmaSmooth = 3
        gaussSigmaSmoothEdge = 4
        sobelThreshold = 0.05       % Threshold for the sobel edge detector. Lowered the default value to detect more edges.
        m_center
        f_elasticity
        c_elasticity = 0.1
        f_image
        c_image = 1
    end
    
    methods
        function addSnakePoint(obj,point)
           obj.snakePoints = [obj.snakePoints point];
        end
        
        function doubleSnakePoints(obj)
            numberOfSnakePoints = numel(obj.snakePoints(1,:));
            oldSnakePoints = obj.snakePoints;
            obj.snakePoints = [];
            
            for i = 1:numberOfSnakePoints
                j = i + 1;
                if(j>numberOfSnakePoints); j = 1; end
                currentPoint = oldSnakePoints(:,i);
                nextPoint = oldSnakePoints(:,j);
                
                newPoint_x = (nextPoint(1)+currentPoint(1)) / 2;
                newPoint_y = (nextPoint(2)+currentPoint(2)) / 2;
                
                obj.snakePoints = [obj.snakePoints currentPoint];
                obj.snakePoints = [obj.snakePoints [newPoint_x;newPoint_y]];
                obj.snakePoints = [obj.snakePoints nextPoint];

            end
        end
        
        function setRawImage(obj,I)
           obj.rawImage = I;
           
           obj.grayImage = rgb2gray(obj.rawImage);
           
           obj.smoothGrayImage = imgaussfilt(obj.grayImage, obj.gaussSigmaSmooth);
           
           obj.smoothGrayEdgeImage = edge(obj.grayImage, 'canny', obj.sobelThreshold);
           obj.smoothGrayEdgeImage = uint8(obj.smoothGrayEdgeImage) .* 255;
           obj.smoothGrayEdgeImage = imgaussfilt(obj.smoothGrayEdgeImage, obj.gaussSigmaSmoothEdge);
           
           obj.setForcesImage(obj.smoothGrayEdgeImage);
        end
        
        function setForcesImage(obj, I)
            obj.forcesImage = I;
        end
        
        function showImage(obj)
            obj.displayImage = obj.rawImage;
            obj.calculateForces();
            
            obj.renderSnakeLines();
            obj.renderSnakePoints();
            obj.renderElasticityForces();
            obj.renderImageForces();
            obj.renderOthers();
            
            imshow(obj.displayImage)
        end
        
        function renderSnakePoints(obj)
            for i = 1:numel(obj.snakePoints(1,:))
                element = obj.snakePoints(:,i);
                obj.displayImage = insertShape(obj.displayImage,'FilledCircle',[element(1) element(2) 5], 'Color','red');
            end
        end
        
        function renderSnakeLines(obj)
            numberOfSnakePoints = numel(obj.snakePoints(1,:));
            for i = 1:numberOfSnakePoints
                j = i + 1;
                if(j>numberOfSnakePoints); j = 1; end
                currentPoint = obj.snakePoints(:,i);
                nextPoint = obj.snakePoints(:,j);
                
                obj.displayImage = insertShape(obj.displayImage,'Line',[currentPoint(1) currentPoint(2) nextPoint(1) nextPoint(2)], 'Color','yellow', 'LineWidth',2);
            end
        end
        
        function renderElasticityForces(obj)
            numberOfSnakePoints = numel(obj.snakePoints(1,:));
            for i = 1:numberOfSnakePoints
                f = obj.f_elasticity(:,i);
                currentPoint = obj.snakePoints(:,i);
                
                obj.displayImage = insertShape(obj.displayImage,'Line',[currentPoint(1) currentPoint(2) currentPoint(1)+f(1) currentPoint(2)+f(2)], 'Color','cyan', 'LineWidth',1);
            end
        end
        
        function renderImageForces(obj)
            numberOfSnakePoints = numel(obj.snakePoints(1,:));
            for i = 1:numberOfSnakePoints
                f = obj.f_image(:,i);
                currentPoint = obj.snakePoints(:,i);
                
                obj.displayImage = insertShape(obj.displayImage,'Line',[currentPoint(1) currentPoint(2) currentPoint(1)+f(1) currentPoint(2)+f(2)], 'Color','magenta', 'LineWidth',1);
            end
        end
        
        function renderOthers(obj)
            obj.displayImage = insertShape(obj.displayImage,'FilledCircle',[obj.m_center(1) obj.m_center(2) 5], 'Color','blue');
        end
        
        function calculateForces(obj)
            obj.calculateCenterOfMass;
            obj.calculateElasticityForces;
            obj.calculateImageForces;
        end
        
        function calculateCenterOfMass(obj)
            numberOfSnakePoints = numel(obj.snakePoints(1,:));
            
            obj.m_center = [0;0];
            for i = 1:numberOfSnakePoints
                obj.m_center(1) = obj.m_center(1) + obj.snakePoints(1,i);
                obj.m_center(2) = obj.m_center(2) + obj.snakePoints(2,i);
            end
            obj.m_center = obj.m_center ./ numberOfSnakePoints;
        end
        
        function calculateElasticityForces(obj)
            numberOfSnakePoints = numel(obj.snakePoints(1,:));
            obj.f_elasticity = [];
            
            for indexCurrent = 1:numberOfSnakePoints
                indexNext = indexCurrent + 1;
                indexPrevious = indexCurrent - 1;
                if(indexNext>numberOfSnakePoints); indexNext = 1; end
                if(indexPrevious<1); indexPrevious = numberOfSnakePoints; end
                
                currentPoint = obj.snakePoints(:,indexCurrent);
                nextPoint = obj.snakePoints(:,indexNext);
                previousPoint = obj.snakePoints(:,indexPrevious);
                
                x_dif_prev = previousPoint(1) - currentPoint(1);
                y_dif_prev = previousPoint(2) - currentPoint(2);
                x_dif_next = nextPoint(1) - currentPoint(1);
                y_dif_next = nextPoint(2) - currentPoint(2);
                
                f_elasticity_x = x_dif_prev + x_dif_next;
                f_elasticity_y = y_dif_prev + y_dif_next;
                
                f_elasticity_x = f_elasticity_x * obj.c_elasticity;
                f_elasticity_y = f_elasticity_y * obj.c_elasticity;
                
                obj.f_elasticity = [obj.f_elasticity [f_elasticity_x;f_elasticity_y]];
            end
        end
        
        function calculateImageForces(obj)
            numberOfSnakePoints = numel(obj.snakePoints(1,:));
            obj.f_image = [];
            imageWidth = numel(obj.rawImage(1,:));
            imageHeight = numel(obj.rawImage(:,1));
            
            for indexCurrent = 1:numberOfSnakePoints
                currentPoint = obj.snakePoints(:,indexCurrent);
                
                indexCurrentPoint_x = uint32(currentPoint(1));
                indexCurrentPoint_y = uint32(currentPoint(2));
                indexPixelRight = indexCurrentPoint_x + 1;
                indexPixelLeft = indexCurrentPoint_x - 1;
                indexPixelTop = indexCurrentPoint_y + 1;
                indexPixelBottom = indexCurrentPoint_y - 1;
                
                if (indexPixelRight > imageWidth); indexPixelRight = imageWidth; end
                if (indexPixelLeft < 1); indexPixelLeft = 1; end
                if (indexPixelTop > imageHeight); indexPixelTop = imageHeight; end
                if (indexPixelBottom < 1); indexPixelBottom = 1; end
                
                f_image_x = obj.forcesImage(indexPixelRight, indexCurrentPoint_y) - obj.forcesImage(indexPixelLeft, indexCurrentPoint_y);
                f_image_y = obj.forcesImage(indexCurrentPoint_x, indexPixelTop) - obj.forcesImage(indexCurrentPoint_x, indexPixelBottom);
                
                f_image_x = f_image_x * obj.c_image;
                f_image_y = f_image_y * obj.c_image;
                
                obj.f_image = [obj.f_image [f_image_x;f_image_y]];
            end
        end
        
        function applyForces(obj)
            obj.calculateForces;
            
            numberOfSnakePoints = numel(obj.snakePoints(1,:));
            for i = 1:numberOfSnakePoints
                currentPoint = obj.snakePoints(:,i);
                currentElasticityForce = obj.f_elasticity(:,i);
                currentImageForce = obj.f_image(:,i);
                
                obj.snakePoints(1,i) = currentPoint(1) + currentElasticityForce(1) + currentImageForce(1);
                obj.snakePoints(2,i) = currentPoint(2) + currentElasticityForce(2) + currentImageForce(2);
            end
        end
        
    end
    
end

