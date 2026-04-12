resource "aws_cloudfront_distribution" "frontend" {
  enabled = true
  default_root_object = "index.html"
  aliases = [var.frontend_domain]

  origin {
    domain_name = aws_s3_bucket.frontend.bucket_regional_domain_name
    origin_id = "omnifit-s3"
    origin_access_control_id = aws_cloudfront_origin_access_control.frontend.id
  }

  default_cache_behavior {
    target_origin_id = "omnifit-s3"
    viewer_protocol_policy = "redirect-to-https"
    allowed_methods = ["GET", "HEAD"]
    cached_methods = ["GET", "HEAD"]

    forwarded_values {
      query_string = false
      cookies {
        forward = "none"
      }
    }
  }

  restrictions {
    geo_restriction {
      restriction_type = "none"
    }
  }
  viewer_certificate {
    acm_certificate_arn = aws_acm_certificate.frontend.arn
    ssl_support_method = "sni-only"
    minimum_protocol_version = "TLSv1.2_2021"
  }

  custom_error_response {
    error_code = 403
    response_code = 200
    response_page_path = "/index.html"
  }
}

resource "aws_cloudfront_origin_access_control" "frontend" {
  name = "omnifit-frontend-oac"
  origin_access_control_origin_type = "s3"
  signing_behavior = "always"
  signing_protocol = "sigv4"
}