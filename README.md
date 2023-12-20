# Shelf Layout Visualization and Modification Tool

## Overview

This project presents a demo of a user-friendly interface for managing SKUs (Stock Keeping Units) on a virtual shelf, utilizing the Blazor framework with C# and .NET version 8.0. It's designed to demonstrate the ability to rapidly learn and apply new technologies in a practical setting. The application features a drag-and-drop functionality for real-time SKU management, enhanced by an advanced service architecture with Dependency Injection and Reactive Programming. This approach ensures a modular and maintainable codebase while prioritizing a responsive and intuitive user experience.

## Features

-   **Shelf Layout Visualization**: Displays a visual representation of the store's shelf layout, highlighting positions of different SKU drinks.
-   **Shelf Layout Modification**: Enables users to add, move, or remove SKU drinks from the shelves. This includes the capability to change the positions of SKUs via a drag-and-drop interface, offering a convenient and intuitive way to rearrange products on the shelf.
-   **Drag-and-Drop Functionality**: Easily reposition SKUs on the shelf with a simple drag-and-drop action.
-   **Reactive Real-Time Updates**: Changes in the SKU placement are instantly reflected, providing a seamless user experience.
-   **Modular Services with Dependency Injection**: Ensures a flexible and scalable application architecture.
-   **Responsive and Accessible Design**: The design adapts to various screen sizes and supports accessibility standards.
-   **Progressive Web Application (PWA) Support**: The app is designed as a Progressive Web Application, making it highly accessible across various devices. It utilizes local storage for efficient data management, allowing offline access and enhanced user experience.

## Target User

The tool is primarily aimed at operations team members with a focus on inventory management and shelf organization. It's built to be intuitive for users with varying technical proficiencies, focusing on operational efficiency and collaboration.

## Data Specification

-   **SKU**: Represents individual items with properties like JanCode, Name, Drink Size, Product Size, Shape Type, Image URL, and Time Stamp.
-   **Shelf**: Represents the physical shelf structure in the store, including Cabinets, Rows, and Lanes with detailed specifications.

## Installation and Setup

1. Clone the repository or extract the zipped file.
2. Navigate to the `shelf-viz-mod` directory.
3. Execute `dotnet restore` to restore all the necessary packages.
4. Run `dotnet build` to build the project.
5. Execute `dotnet run` to start the application.
6. Access the app at [http://localhost:5290](http://localhost:5290/)

## Testing

The `shelf-viz-mod.Tests` directory contains unit tests for various components and services. To run these tests:

-   Navigate to `shelf-viz-mod.Tests`
-   Execute `dotnet test`

## Future Improvement Plan

### User Experience Improvements:

-   **Undo/Redo Feature**: Introduce an undo/redo system for easy reversal or reapplication of changes. _(Nice to Have)_
-   **Enhanced Drag-and-Drop Interface**: Upgrade the drag-and-drop experience for improved interaction. _(Nice to Have)_

### Refactoring:

-   **Removal of Hardcoded Values**: Shift to configurable options for enhanced flexibility. _(Must Happen)_

### Data Handling and Storage:

-   **Transition to Backend API**: Move from local storage to a backend API for scalability. _(Must Happen)_
-   **SignalR for Real-Time Updates**: Implement SignalR for synchronized real-time data updates. _(Must Happen)_

### Additional Features:

-   **Customizable Shelf Layouts**: Enable users to create and save various shelf layouts. _(Must Happen)_
-   **Advanced Filtering and Search**: Offer sophisticated search and filtering tools. _(Must Happen)_
-   **Integration with Inventory Management Systems**: Facilitate integration with existing systems. _(Must Happen)_
-   **Responsive and Accessible UI/UX Design**: Ensure the design is responsive and accessible. _(Nice to Have)_

### Code Quality and Documentation:

-   **Code Documentation and Comments**: Improve documentation for better collaboration. _(Must Happen)_
-   **Automated Testing**: Expand testing to cover more scenarios. _(Must Happen)_

### UI/UX Enhancements:

-   **Visual Indicator for Empty Slots**: Use distinct styles to indicate where new SKUs can be added. _(Nice to Have)_
-   **Add SKU Button**: Simplify adding SKUs with a dedicated button or panel. _(Nice to Have)_
-   **Drag to Add**: Enhance drag-and-drop for adding SKUs from a catalog. _(Nice to Have)_
-   **Hover Effects**: Implement interactive hover effects for user guidance. _(Nice to Have)_
-   **Editable SKU Components**: Allow easy modification of SKU details through interactive components. _(Nice to Have)_
-   **Hero Transition for SKU Editing**: Utilize visually appealing transitions for SKU editing interfaces. _(Nice to Have)_
-   **Confirm Deletion**: Include a confirmation step to prevent accidental SKU deletions. _(Nice to Have)_

## Development Time

-   Read Documents: 30 mins
-   Initial Design and Configuration: 4 hours
-   Core Feature Implementation: 10 hours
-   User Interface Design and UX Improvements: 4 hours
-   Integration and Testing: 2 hours
-   Documentation and Final Review: 1 hour
-   **Total Estimated Time**: 21.5 hours
