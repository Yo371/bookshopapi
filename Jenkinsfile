node {
  stage('SCM') {
    checkout scm
  }
  stage('SonarQube Analysis') {
    def scannerHome = tool 'SonarScanner for MSBuild'
    withSonarQubeEnv() {
      bat "dotnet ${scannerHome}\\SonarScanner.MSBuild.dll begin /k:\"bookshop\""
      bat "dotnet build"
      bat "dotnet ${scannerHome}\\SonarScanner.MSBuild.dll end"
    }
  }
  stage('Build Stage') {
    bat 'dotnet build'
  }
  stage('Test Stage') {
    bat 'dotnet test --filter FullyQualifiedName~Tests.UnitTests.ValidationRole'
  }
}
