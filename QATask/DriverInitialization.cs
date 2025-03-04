﻿using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace QATask;

public class DriverInitialization
{
    private readonly string _baseUrl = Environment.GetEnvironmentVariable("url") ?? "https://shorturl.at/PTVHg";
    protected Actions Actions;
    protected Waits Waits;
    protected IWebDriver Driver { get; private set; }

    [SetUp]
    public void Setup()
    {
        new DriverManager().SetUpDriver(new FirefoxConfig());

        Driver = new FirefoxDriver();
        Waits = new Waits(Driver);
        Actions = new Actions(Driver, Waits);

        StartBrowser();
    }

    [TearDown]
    public void TearDown()
    {
        Driver.Quit();
        Driver.Dispose();
    }

    private void StartBrowser()
    {
        Driver.Navigate().GoToUrl(_baseUrl);
    }
}