# Shelf Layout Visualization and Modification Tool

## Overview

This document outlines the Shelf Layout Visualization and Modification Tool, a technical demonstration developed to showcase proficiency in Blazor and .Net. The application serves as a testament to the ability to rapidly learn and effectively utilize technology in a practical setting. This tool, designed with local storage, serves as an example of a Progressive Web Application (PWA). Its implementation allows for efficient operation and utilization of local storage features. The primary goal is to assist operations teams in effectively visualizing and adjusting shelf layouts for SKU drinks in retail settings.

## Target User

The tool is primarily aimed at operations team members with a focus on inventory management and shelf organization. It's built to be intuitive for users with varying technical proficiencies, focusing on operational efficiency and collaboration.

## Key Features

-   **Shelf Layout Visualization**: Displays a visual representation of the store's shelf layout, highlighting positions of different SKU drinks.
-   **Shelf Layout Modification**: Enables users to add, move, or remove SKU drinks from the shelves. This includes the capability to change the positions of SKUs via a drag-and-drop interface, offering a convenient and intuitive way to rearrange products on the shelf.

## Data Specification

-   **SKU**: Represents individual items with properties like JanCode, Name, Drink Size, Product Size, Shape Type, Image URL, and Time Stamp.
-   **Shelf**: Represents the physical shelf structure in the store, including Cabinets, Rows, and Lanes with detailed specifications.

## Technical Requirements

-   Frontend developed using Blazor.
-   Reactive Programming implemented for real-time data updates.
-   Dependency Injection for service management.
-   Application structured following Clean Architecture principles.

## Installation and Setup

1. Clone the repository or extract the zipped file.
2. Navigate to the `shelf-viz-mod` directory.
3. Execute `dotnet run` to start the application.

## Testing

The `shelf-viz-mod.Tests` directory contains unit tests for various components and services. To run these tests:

-   Navigate to `shelf-viz-mod.Tests`
-   Execute `dotnet test`

## Future Improvement Plan

[...Future Improvement Plan Content...]

## Development Time

-   Read Documents: 30 mins
-   Initial Design and Configuration: 4 hours
-   Core Feature Implementation: 10 hours
-   User Interface Design and UX Improvements: 4 hours
-   Integration and Testing: 2 hours
-   Documentation and Final Review: 1 hour
-   **Total Estimated Time**: 21.5 hours
