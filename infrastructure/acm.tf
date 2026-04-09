resource "aws_acm_certificate" "frontend" {
  provider = aws.us_east_1
  domain_name = var.frontend_domain
  validation_method = "DNS"
}