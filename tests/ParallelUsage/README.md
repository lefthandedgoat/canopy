# Parallel usage of Canopy

This is a sample project that showcases how you can write your UI tests in total bliss with Canopy
and Expecto together. There are many upsides of doing it this way:

 - Expecto is parallel by default
 - Expecto supports FsCheck and can generate input for your pages
 - Expecto is configurable both via command line and environment variables
 - Expecto can run stress tests; where the FsCheck input is continously varied for a specified amount of time.
 - Expecto supports custom loggers, so you can output into whatever reporting system is your favourite.
 
## Getting started

 1. Download the geckodriver (`./download-runner.sh`)
 1. Compile this solution (`./build.sh`)
 1. Run these tests (`mono --debug bin/Debug/Canopy.ParallelUsage.exe`) and look at output.
 1. Start implementing tests; open up `Explore.fsx`, and read the below docs:
 
## Usage & code organisation

 - `Expecto.fs` — by shadowing part of Expecto's API, we can make the DSL for Canopy even nicer to use;
   these functions handle the starting and stopping of your browser instance.
 - `Config.fs` — as your test suite grows, you'll need to add configuration to it, so that you can focus/vary
   and interact with your code in a way that's productive for you as a developer or QA specialist.
 - `Tests.fs` — provides a listing of the interaction tests to run
 - `Pages.fs` — here's where you write your Canopy-based interactions with the site:
    1. Start by opening `Explore.fsx` and running the imports in `fsharpi`: this will give your interactive
       a baseline state to work with and an open browser at the right URL.
    1. Now, you run code from `Pages.fs` while having the *Web Developer Console* open, finding
       selectors as you go. You can type straight into `Pages.fs` and evaluate line by line into the
       interactive; this keeps you productive.
    1. When you think you've finished a page/use-case/interaction, just call your `Pages`-function from
       a test in `Tests.fs`. Now you have integration tests!
 - `Program.fs` — the main entry point, which handles argument parsing, handles
   SIGINT-based cancellation and delegates the bulk of the work to Expecto to run.


## A note on editors

The best interactive F# experience can be had with VSCode and [Ionide][i]. Ionide also
supports Expecto out of the box and has the best F# Interactive integration.

 [i]: http://ionide.io/