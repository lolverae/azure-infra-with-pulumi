## About
Infrastructure as Code for creating Azure resources on a project running mainly on an AKS cluster with Pulumi using C#.

## Project Structure
This project is created with a monorepo pattern in mind, where everything related to the AKS clustergets stored in a single project for an entire Azure service

This Pulumi project will use a stack per each environment, following a dev -> QA -> production workflow.

