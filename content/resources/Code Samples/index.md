---
layout: layout-container
showHeadings: false
order: 10
name: Code Samples
description: A listing of code samples for the UAT Automation Kit walkthroughs.
published: true
code:
  - title: NEW VERSION Get Started with the UAT Automation Kit
    description: Use this sample to learn how to create a test project and get started on the path to creating a ***Blackbaud CRM*** test suite with the UAT Automation Kit.
    download: /assets/code-samples/GetStarted/GetStarted.zip
    tutorial: <%= stache.config.blue_walkthroughs %>getting-started/

  - title: Delving Deeper with the UAT Automation Kit
    description: Use this sample to learn how to use the UAT Automation Kit's underlying third-party tools to create custom logic and interactions with the browser.
    download: /assets/code-samples/DelvingDeeper/DelvingDeeper.zip
    tutorial: <%= stache.config.blue_walkthroughs %>selenium/
---

{{ include 'includes/eapwarning/index.md' }}

# UAT Automation Kit Code Samples
<div class="code">

  <div class="clearfix"></div>

  {{# eachWithMod code mod=3 }}

    {{# if firstOrMod0 }}
      <div class="row">
    {{/ if }}
        <div class="col-sm-6 col-md-4">
          <div class="thumbnail">
             {{# if itemimage }}
               <img src="{{ itemimage }}" alt="" />
            {{/ if }}
            <div class="caption">
              <h3>{{ title }}</h3>
               {{# if description }}
                <p>{{ description }}</p>
              {{/ if }}
              <a href="{{download}}" class="btn btn-primary" role="button" download>Download Code</a>
              {{# if tutorial }}
                <a href="{{ tutorial }}" class="btn btn-white">
                  View Tutorial
                </a>
              {{/ if }}
            </div>
          </div>
        </div>
    {{# if lastOrMod1 }}
      </div>
    {{/ if }}

  {{/ eachWithMod }}

</div>

<!--
## test download -- commented out instead of deleted just in case I need to return to see how these links worked
Old 201
<a href="/assets/code-samples/201/ProjectBlue201.zip" download>Test Download</a>
<a href="https://github.com/blackbaud/blue-docs/blob/master/static/assets/code-samples/201/ProjectBlue201.zip?raw=true">Test Link</a>

Get Started
<a href="/assets/code-samples/GetStarted/GetStarted.zip" download>Test Download</a>
<a href="https://github.com/blackbaud/blue-docs/blob/master/static/assets/code-samples/GetStarted/GetStarted.zip?raw=true">Test Link</a>

Delving Deeper
<a href="/assets/code-samples/DelvingDeeper/DelvingDeeper.zip" download>Test Download</a>
<a href="https://github.com/blackbaud/blue-docs/blob/master/static/assets/code-samples/DelvingDeeper/DelvingDeeper.zip?raw=true">Test Link</a>
-->
